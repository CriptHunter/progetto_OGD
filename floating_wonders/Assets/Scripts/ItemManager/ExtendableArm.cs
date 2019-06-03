using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExtendableArm : NetworkBehaviour
{
    private LayerMask ignoredLayer = ~((1 << 2) | (1 << 9));
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private float maxDistance = 10f;  //massima distanza del braccio
    private RaycastHit2D hit; //oggetto da raccogliere
    private State state;
    private Rigidbody2D hitRb;

    private enum State : int
    {
        none = 0,
        anchored = 1,
        missed = 2,
    }

    private void Start()
    {
        state = State.none;
    }

    private void FixedUpdate()
    {
        if (state == State.anchored)
        {
            print("distanza dal giocatore: " + Vector2.Distance(hit.transform.position, transform.position));
            print("velocità: " + hitRb.velocity.magnitude);
            //se la cassa si incastra in qualcosa
            if (hitRb.velocity.magnitude < 0.5f)
                state = State.none;
            if (Vector2.Distance(hit.transform.position, transform.position) < 3f)
            {
                state = State.none;
                hitRb.velocity = Vector2.zero;
            }
            if(state == State.anchored)
                hitRb.velocity = (firePoint.position - hit.transform.position).normalized * 20;
        }
    }

    public void Throw(Vector2 direction)
    {
        hit = Physics2D.Raycast(firePoint.position, direction, maxDistance, ignoredLayer);
        if (hit.collider != null)
        {
            //raccoglie collezionabili o oggetti
            if (hit.transform.GetComponent<Pickuppable>() != null)
                GetComponent<ItemManager>().Pickup(hit.collider.gameObject);
            //trascina verso di se una cassa
            else if (hit.collider.tag == "Crate")
            {
                state = State.anchored;
                hitRb = hit.collider.GetComponent<Rigidbody2D>();
                hitRb.velocity = (firePoint.position - hit.transform.position).normalized * 20;
            }
        }
    }
}
