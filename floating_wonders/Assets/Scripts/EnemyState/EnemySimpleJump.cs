using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleJump : MonoBehaviour
{
    public float speed;
    public float distance;

    private bool movingRight = true;
    private LayerMask playerMask;
    private LayerMask groundMask;
    private Rigidbody2D rigidbody;
    public Transform groundDetection;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float horizzontalMovement;
    [SerializeField]
    private float jumpWaitingTime;
    private float timer;
    private bool grounded;

    private void Start()
    {
        playerMask = LayerMask.NameToLayer("Player");
        groundMask = LayerMask.NameToLayer("Ground");
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        //transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance + jumpForce);
        if (groundInfo.collider == false)
        {
            Debug.Log("FloorEnded");
            ChangeDirection();
        }
        if (grounded)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
            if (timer > jumpWaitingTime)
            {
                Jump();
                timer = 0;
            }
        }
    }

    private void Jump()
    {
        //transform.Translate(Vector2.right * speed * Time.deltaTime);
        //rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position + new Vector3(horizzontalMovement,0,0), Vector2.down, distance + jumpForce);
        if (groundInfo.collider != false)
        {
            rigidbody.AddForce(new Vector2(horizzontalMovement, jumpForce), ForceMode2D.Impulse);
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
            horizzontalMovement = -1 * horizzontalMovement;
            movingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            horizzontalMovement = -1 * horizzontalMovement;
            movingRight = true;
        }
    }

    // Sent when another object enters a trigger collider attached to this object
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision with something not ground");
        if (collision.transform.gameObject.layer == groundMask)
        {
            Debug.Log("Collision with ground");
            grounded = true;
            
        }
        
    }
}
