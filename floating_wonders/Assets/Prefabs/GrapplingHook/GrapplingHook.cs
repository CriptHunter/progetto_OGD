using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrapplingHook : NetworkBehaviour
{
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private LineRenderer line; //fune del rampino
    [SerializeField] private float maxDistance = 10f;  //massima distanza della fune del rampino
    [SerializeField] private float breakDistance = 1f; //distanza minima dal punto di aggancio, se la distanza diventa minore il rampino si sgancia
    [SerializeField] private float step = 0.2f; //di quanto si accorcia la corda ogni volta
    private DistanceJoint2D joint; //linea che collega il giocatore con il punto di attacco
    private RaycastHit2D hit;
    private bool anchored; //true se il personaggio è attaccato con il rampino

    private void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        line.enabled = false;
    }

    public void Update()
    {
        if (anchored)
        {
            Cmd_DrawLine(true, firePoint.position, hit.point);
            if (joint.distance > breakDistance)
                joint.distance = joint.distance - step;
            else
            {
                Cmd_DrawLine(false, firePoint.position, hit.point);
                this.gameObject.GetComponent<AnotherCharacterController>().Activate(true);
                joint.enabled = false;
                anchored = false;
            }
        }
    }

    public void Throw(Vector2 direction)
    {
        print(direction);
        hit = Physics2D.Raycast(firePoint.position, direction, maxDistance);
        if (hit.collider != null && hit.transform.gameObject.tag == "HookPoint")
        {
            anchored = true;
            joint.enabled = true;
            joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
            joint.distance = Vector2.Distance(firePoint.position, hit.point);
            this.gameObject.GetComponent<AnotherCharacterController>().Deactivate(true);
            Cmd_DrawLine(true, firePoint.position, hit.point);
        }
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
