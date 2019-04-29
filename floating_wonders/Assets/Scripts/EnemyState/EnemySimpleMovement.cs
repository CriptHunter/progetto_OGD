using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class EnemySimpleMovement : NetworkBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float groundRayDistance;
    /*[SerializeField]
    private bool followPlayer;
    private float sightSee;*/

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
        /*if (followPlayer)
            FollowP();*/
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

    /*private void FollowP()
    {
        RaycastHit2D verticalRay;
        RaycastHit2D horizzontalRay;

        verticalRay = Physics2D.Raycast(transform.position, Vector2.up, sightSee, playerMask);
        if (movingRight)
        {
            horizzontalRay = Physics2D.Raycast(transform.position, Vector2.right, sightSee, playerMask);

            Debug.DrawRay(transform.position, Vector2.right * sightSee, Color.green);
        }
        else
        {
            horizzontalRay = Physics2D.Raycast(transform.position, Vector2.left, sightSee, playerMask);
            Debug.DrawRay(transform.position, Vector2.left * sightSee, Color.green);
        }

        if (horizzontalRay.collider)
            playerRight = true;
        else
            playerRight = false;
        if (verticalRay.collider)
        {
            playerRight = !playerRight;
            Debug.DrawRay(transform.position, Vector2.up * sightSee, Color.green);
        }
    }*/

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
