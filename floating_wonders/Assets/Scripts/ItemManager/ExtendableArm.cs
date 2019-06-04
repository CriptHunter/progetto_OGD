using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExtendableArm : NetworkBehaviour
{
    private LayerMask ignoredLayer = ~((1 << 2) | (1 << 9));
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer line; //braccio disegnato con una linea
    [SerializeField] private float maxDistance; //distanza massima del braccio
    [SerializeField] private float grabSpeed;
    private RaycastHit2D hit; //oggetto da raccogliere
    private State state;
    private Rigidbody2D hitRb;
    private Vector2 hitPosition;
    private Vector2 direction;

    private enum State : int
    {
        none = 0,
        anchoredToCrate = 1,
        anchoredToItem = 2,
        missed = 3,
    }

    private void Start()
    {
        state = State.none;
    }

    private void FixedUpdate()
    {
        if (state == State.anchoredToCrate)
        {
            print("distanza dal giocatore: " + Vector2.Distance(hit.transform.position, transform.position));
            print("velocità: " + hitRb.velocity.magnitude);
            //se la cassa si incastra in qualcosa o arriva al giocatore
            if (Vector2.Distance(hit.transform.position, transform.position) < 3f || hitRb.velocity.magnitude < 0.5f)
            {
                state = State.none;
                hitRb.velocity = Vector2.zero;
                Cmd_DrawLine(false, firePoint.position, hit.transform.position);
            }
            if (state == State.anchoredToCrate)
            {
                Cmd_DrawLine(true, firePoint.position, hit.transform.position);
                hitRb.velocity = (firePoint.position - hit.transform.position).normalized * grabSpeed;
            }
        }
        if(state == State.anchoredToItem)
        {
            hitPosition = Vector3.Lerp(firePoint.position, hitPosition, 0.92f);
            Cmd_DrawLine(true, firePoint.position, hitPosition);
            if (Vector2.Distance(firePoint.position, hitPosition) < 2)
            {
                state = State.none;
                Cmd_DrawLine(false, Vector2.zero, Vector2.zero);
            }
        }

        if(state == State.missed)
        {
            print("missed");
        }
    }

    public void Throw(Vector2 direction)
    {
        this.direction = direction;
        hit = Physics2D.Raycast(firePoint.position, direction, maxDistance, ignoredLayer);
        if (hit.collider != null)
        {
            //raccoglie collezionabili o oggetti
            if (hit.transform.GetComponent<Pickuppable>() != null)
            {
                GetComponent<ItemManager>().Pickup(hit.collider.gameObject);
                state = State.anchoredToItem;
                hitPosition = hit.transform.position;
            }
            //trascina verso di se una cassa
            else if (hit.collider.tag == "Crate")
            {
                state = State.anchoredToCrate;
                hitRb = hit.collider.GetComponent<Rigidbody2D>();
                hitRb.velocity = (firePoint.position - hit.transform.position).normalized * grabSpeed;
                Cmd_DrawLine(true, firePoint.position, hit.transform.position);
            }
            else
                state = State.missed;
        }
        else
            state = State.missed;
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
