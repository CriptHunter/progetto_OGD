using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendableArm : MonoBehaviour
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
                Vector2 forceVector = this.gameObject.transform.position - hit.transform.position;
                hit.transform.gameObject.GetComponent<Rigidbody2D>().AddForce(grabForce * forceVector);
            }
            else
            { 
                grabbed = false;
                this.gameObject.GetComponent<AnotherCharacterInput>().enabled = true;
            }
        }
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
