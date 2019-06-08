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
    private LayerMask playerMask;
    private float timer;

    private void Start()
    {
        ChangeDirection(movingRight);
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        playerMask = LayerMask.NameToLayer("Player");
    }

    void FixedUpdate()
    {
        if (!isServer)
            return;
        //se sta cadendo
        if (GetComponent<Rigidbody2D>().velocity.y < 0)
            return;

        timer += Time.deltaTime;
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D forwardHit;
        if (movingRight)
            forwardHit = Physics2D.Raycast(forwardDetection.position, transform.right, 1, groundMask);
        else
            forwardHit = Physics2D.Raycast(forwardDetection.position, -transform.right, 1, groundMask);
        RaycastHit2D downwardHit = Physics2D.Raycast(groundDetection.position, Vector2.down, groundRayDistance, groundMask);
        if (forwardHit.collider != null)
            this.movingRight = !movingRight;
        else if (!downwardHit && !flying)
            this.movingRight = !movingRight;
    }

    public override void ChangeDirection(bool movingRight)
    {
        //per evitare che il nemico si giri in continuazione
        // controllo il timer solo sul server perchè quando questo metodo è chiamato sul client significa che il nemico deve girarsi davvero
        if (isServer && timer < 0.5f)
            return;

            if (movingRight)
                transform.eulerAngles = new Vector3(0, -180, 0);
            else
                transform.eulerAngles = new Vector3(0, 0, 0);
            this.movingRight = movingRight;
            timer = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.layer == playerMask)
            this.movingRight = !movingRight;
    }
}
