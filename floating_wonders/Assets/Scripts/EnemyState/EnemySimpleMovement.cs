using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class EnemySimpleMovement : EnemyBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float groundRayDistance;
    [SerializeField] private bool flying;
    [SerializeField] private Transform groundDetection;
    [SerializeField] private Transform forwardDetection;

    private LayerMask groundMask;
    //private LayerMask ignoredLayer = ~((1 << 2) | (1 << 9) | (1 << 10) | (1 << 13));

    private void Start()
    {
        ChangeDirection(movingRight);
        groundMask = 1 << LayerMask.NameToLayer("Ground");
    }

    void FixedUpdate()
    {
        if (!isServer)
            return;

        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D forwardHit;
        if (movingRight)
            forwardHit = Physics2D.Raycast(forwardDetection.position, transform.right, 1, groundMask);
        else
            forwardHit = Physics2D.Raycast(forwardDetection.position, -transform.right, 1, groundMask);
        RaycastHit2D downwardHit = Physics2D.Raycast(groundDetection.position, Vector2.down, groundRayDistance, groundMask);
        if (forwardHit.collider != null)
        {
            this.movingRight = !movingRight;
            print("sto colpendo davanti " + forwardHit.collider.gameObject);
        }
        else if (!downwardHit && !flying)
        {
            this.movingRight = !movingRight;
            print("sotto di me c'è solo il vuoto");
        }
    }

    public override void ChangeDirection(bool movingRight)
    {
        if (movingRight)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            this.movingRight = movingRight;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            this.movingRight = movingRight;
        }
    }

}
