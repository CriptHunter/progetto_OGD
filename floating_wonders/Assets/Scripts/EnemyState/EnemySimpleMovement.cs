using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class EnemySimpleMovement : NetworkBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float groundRayDistance;
    [SerializeField] bool flyingEnemy;


    private bool movingRight = true;
    private LayerMask playerMask;
    private LayerMask groundMask;
    private LayerMask groundCheckMask;

    public Transform groundDetection;

    private bool playerRight;


    private void Start()
    {
        playerMask = LayerMask.NameToLayer("Player");
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        groundCheckMask = LayerMask.NameToLayer("Ground");
    }

    void Update()
    {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            //This will return true only if the ray hit an object of the Ground layer. 
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, groundRayDistance, groundMask);
            if (!groundInfo)
            {
                Debug.Log("FloorEnded");
                ChangeDirection();
            }
            else
                Debug.Log("Nope");
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


    // Sent when another object enters a trigger collider attached to this object
    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the enemy collide with a Player, hurt him and change direction
        if (collision.transform.gameObject.layer == playerMask)
        {
            Debug.Log("Collision with player");
            //Eventually damage the player
            ChangeDirection();
        }
        else if (collision.transform.gameObject.layer != groundCheckMask)
        {
            Debug.Log("Collision with something not player:" + collision.collider.name);
            ChangeDirection();
        }
        else
        {
            Debug.Log("Collision with Ground");
        }
    }
}
