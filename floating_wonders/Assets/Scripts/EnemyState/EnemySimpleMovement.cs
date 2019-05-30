using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class EnemySimpleMovement : NetworkBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float groundRayDistance;
    [SerializeField] private bool flying;

    private bool movingRight = true;
    private LayerMask groundMask;
    private LayerMask groundCheckMask;
    public Transform groundDetection;

    private void Start()
    {
        groundMask = 1 << LayerMask.NameToLayer("Ground");
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        if (!flying)
        {
            RaycastHit2D hit = Physics2D.Raycast(groundDetection.position, Vector2.down, groundRayDistance, groundMask);
            if (!hit)
                ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        if (movingRight)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeDirection();
    }
}
