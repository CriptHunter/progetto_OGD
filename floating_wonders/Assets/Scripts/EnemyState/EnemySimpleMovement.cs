using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class EnemySimpleMovement : EnemyBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float groundRayDistance;
    [SerializeField] private bool flying;

    private LayerMask groundMask;
    private LayerMask groundCheckMask;
    public Transform groundDetection;

    private void Start()
    {
        ChangeDirection(movingRight);
        groundMask = 1 << LayerMask.NameToLayer("Ground");
    }

    void FixedUpdate()
    {
        if (isServer)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (!flying)
            {
                RaycastHit2D hit = Physics2D.Raycast(groundDetection.position, Vector2.down, groundRayDistance, groundMask);
                if (!hit)
                    this.movingRight = !movingRight;
            }
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        this.movingRight = !movingRight;
    }
}
