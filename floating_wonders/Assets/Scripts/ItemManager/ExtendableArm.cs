using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExtendableArm : NetworkBehaviour
{
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private float maxDistance = 30f;  //massima distanza del braccio
    [SerializeField] private float grabForce = 30f; //con quanta forza trascina il bersaglio verso di se
    private RaycastHit2D hit; //oggetto da trascinare
    private bool grabbed; //true se il personaggio è attaccato con il braccio
    private bool justGrabbed; //se ho appena agganciato il bersaglio
    private int i = 0;

    private void Start()
    {
        grabbed = false;
        justGrabbed = true;
    }

    public void Update()
    {
        if (grabbed)
        {
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), hit.transform.position) > 2 &&
                (hit.transform.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > 0.5 || justGrabbed))
            {
                justGrabbed = false;
                Vector2 forceVector = firePoint.transform.position - hit.transform.position;
                Cmd_AddForce(hit.transform.gameObject, forceVector, grabForce);
            }
            else
            {
                grabbed = false;
                this.gameObject.GetComponent<AnotherCharacterInput>().enabled = true;
            }
        }
    }

    [Command] private void Cmd_AddForce(GameObject obj, Vector2 direction, float force)
    {
        obj.GetComponent<Rigidbody2D>().AddForce(force * direction);
    }

    public void Throw(Vector2 direction)
    {
        hit = Physics2D.Raycast(firePoint.position, direction, maxDistance);
        if (hit.collider != null && hit.transform.GetComponent<Rigidbody2D>() != null)
        {
            grabbed = true;
            justGrabbed = true;
            this.gameObject.GetComponent<AnotherCharacterInput>().enabled = false;
        }
    }
}
