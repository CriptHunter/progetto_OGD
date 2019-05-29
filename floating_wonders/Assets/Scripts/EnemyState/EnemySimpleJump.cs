using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySimpleJump : NetworkBehaviour
{
    //public float speed;

    private bool movingRight = true;
    private LayerMask groundMask;
    private LayerMask ignoredLayer = ~((1 << 9) | (1 << 10) | (1 << 13));
    private Rigidbody2D rigidbody;
    public Transform groundDetection;
    [SerializeField] private float jumpForce;
    [SerializeField] private float horizontalMovement;
    [SerializeField] private float jumpWaitingTime;
    [SerializeField] float distance;

    private float timer;
    private bool grounded;

    private void Start()
    {
        groundMask = LayerMask.NameToLayer("Ground");
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (grounded)
        {
            RaycastHit2D downwardHit = Physics2D.Raycast(groundDetection.position + new Vector3(horizontalMovement, 0, 0), Vector2.down);
            RaycastHit2D forwardHit = Physics2D.Raycast(transform.position, transform.right, horizontalMovement, ignoredLayer);
            if (forwardHit.collider != null)
                ChangeDirection();
            else if (downwardHit.collider == null)
                ChangeDirection();

            timer += Time.deltaTime;
            if (timer > jumpWaitingTime)
            {
                Jump();
                timer = 0;
            }
        }
    }

    private void Jump()
    {
        rigidbody.AddForce(new Vector2(horizontalMovement, jumpForce), ForceMode2D.Impulse);
        grounded = false;
    }

    private void ChangeDirection()
    {
        if (movingRight)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            horizontalMovement = -1 * horizontalMovement;
            movingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            horizontalMovement = -1 * horizontalMovement;
            movingRight = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.layer == groundMask)
            grounded = true;
        else if (collision.gameObject.GetComponent<Pickuppable>().Type == ItemType.player)
            ChangeDirection();
    }
}
