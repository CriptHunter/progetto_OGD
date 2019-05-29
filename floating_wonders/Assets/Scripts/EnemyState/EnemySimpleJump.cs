using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySimpleJump : NetworkBehaviour
{
    public float speed;
    public float distance;

    private bool movingRight = true;
    private LayerMask groundMask;
    private Rigidbody2D rigidbody;
    public Transform groundDetection;
    [SerializeField] private float jumpForce;
    [SerializeField] private float horizontalMovement;
    [SerializeField] private float jumpWaitingTime;
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
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position + new Vector3(horizontalMovement, 0, 0), Vector2.down, 1);
            Debug.DrawLine(groundDetection.position, Vector2.down);
            if (groundInfo.collider == false)
            {
                Debug.Log("FloorEnded");
                ChangeDirection();
            }

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
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position + new Vector3(horizontalMovement,0,0), Vector2.down, distance + jumpForce, (1 << 8));
        if (groundInfo.collider != null)
        {
            rigidbody.AddForce(new Vector2(horizontalMovement, jumpForce), ForceMode2D.Impulse);
            grounded = false;
        }
        else
            ChangeDirection();
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

    // Sent when another object enters a trigger collider attached to this object
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.gameObject.layer == groundMask)
            grounded = true;
        else if (collision.gameObject.GetComponent<Pickuppable>().Type == ItemType.player)
            ChangeDirection();
    }
}
