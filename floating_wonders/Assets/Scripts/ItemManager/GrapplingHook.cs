using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrapplingHook : NetworkBehaviour
{
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private LineRenderer line; //fune del rampino
    [SerializeField] private float maxDistance = 13f;  //massima distanza della fune del rampino
    [SerializeField] private float breakDistance = 1f; //distanza minima dal punto di aggancio, se la distanza diventa minore il rampino si sgancia
    [SerializeField] private float step = 0.2f; //di quanto si accorcia la corda ogni volta
    private DistanceJoint2D joint; //linea che collega il giocatore con il punto di attacco
    private AnotherCharacterController controller;
    private ItemManager itemManager;
    private RaycastHit2D hit;
    private Vector3 missedPoint; //punto in cui ha missato il rampino
    private State state; //true se il personaggio è attaccato con il rampino
    private LayerMask ignoredLayer = ~((1 << 2) | (1 << 9) | (1 << 11) | (1 << 12) | (1 << 13) | (1 << 16));

    private enum State : int
    {
        none = 0,
        anchored = 1,
        missed = 2,
    }

    private void Start()
    {
        controller = GetComponent<AnotherCharacterController>();
        itemManager = GetComponent<ItemManager>();
        state = State.none;
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        line.enabled = false;
    }

    public void Update()
    {
        if (state == State.anchored)
        {
            Cmd_DrawLine(true, firePoint.position, hit.point);
            if (joint.distance > breakDistance)
                joint.distance = joint.distance - step;
            /*if (Vector2.Distance(transform.position, hit.point) > breakDistance)
                joint.distance = joint.distance - step;*/
            else
            {
                Cmd_DrawLine(false, firePoint.position, hit.point);
                controller.Activate(true);
                GetComponent<AnotherCharacterInput>().enabled = true;
                itemManager.enabled = true;
                joint.enabled = false;
                state = State.none;
            }
        }
    }

    //lancia il rampino in direzione direction
    public void Throw(Vector2 direction)
    {
        hit = Physics2D.Raycast(firePoint.position, direction, maxDistance, ignoredLayer);
        //se colpisco un hook point, e non sto usando il rampino e sono a terra --> si attacca al punto
        if (hit.collider != null && hit.transform.gameObject.tag == "HookPoint" && state == State.none)
        {
            Turn(hit.point);
            state = State.anchored;
            joint.enabled = true;
            joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
            joint.distance = Vector2.Distance(firePoint.position, hit.point);
            controller.Deactivate(true);
            GetComponent<AnotherCharacterInput>().enabled = false;
            itemManager.enabled = false;
            Cmd_DrawLine(true, firePoint.position, hit.point);
        }
        else
        {
            state = State.missed;
            print("missed");
            state = State.none;
        }
    }

    private void Turn(Vector2 direction)
    {
        //se sparo verso sinistra e il personaggio è girato a destra ---> lo giro verso destra
        if (direction.x < this.transform.position.x && controller.GetVerse() == Verse.Right)
            controller.Turn(Verse.Left);
        //se sparo verso destra e il personaggio è girato a sinistra
        else if (direction.x > this.transform.position.x && controller.GetVerse() == Verse.Left)
            controller.Turn(Verse.Right);
    }

    public float GetMaxDistance()
    {
        return this.maxDistance;
    }

    [Command]
    private void Cmd_DrawLine(bool enabled, Vector3 start, Vector3 end)
    {
        Rpc_DrawLine(enabled, start, end);
    }

    [ClientRpc]
    private void Rpc_DrawLine(bool enabled, Vector3 start, Vector3 end)
    {
        line.enabled = enabled;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}
