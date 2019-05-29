using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class EnemySimpleMovement : NetworkBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float groundRayDistance;
    [SerializeField] bool utnilEndPlatform;
    [SerializeField] private Vector2 directionVector;


    private bool leftOrRight = true;
    private bool upOrDown = true;
    private LayerMask playerMask;
    private LayerMask groundMask;
    private LayerMask groundCheckMask;

    public Transform groundDetection;

    [SerializeField] private bool flyingEnemy;

    public Transform upperDetection;
    public Transform lowerDetection;

    private bool playerRight;

    private Vector3 startingPos;
    private Vector3 directionVector3;


    private void Start()
    {
        playerMask = LayerMask.NameToLayer("Player");
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        groundCheckMask = LayerMask.NameToLayer("Ground");
        startingPos = transform.position;
    }

    void Update()
    {
        if (utnilEndPlatform)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            //This will return true only if the ray hit an object of the Ground layer. 
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, groundRayDistance, groundMask);
            if (!groundInfo)
            {
                Debug.Log("FloorEnded");
                ChangeLeftOrRight();
            }
            else
                Debug.Log("Nope");
        }
        else
        {
            MoveInDirection();
        }
    }

    private void MoveInDirection ()
    {
        if (!isServer)
            return;
        directionVector3 = new Vector3(directionVector.x, directionVector.y, 0);
        if (transform.position == startingPos + directionVector3 * groundRayDistance)
            upOrDown = false;
        if (transform.position == startingPos - directionVector3 * groundRayDistance)
            upOrDown = true;

        if (upOrDown)
            transform.position = Vector2.MoveTowards(transform.position, startingPos + directionVector3 * groundRayDistance, speed * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(transform.position, startingPos - directionVector3 * groundRayDistance, speed * Time.deltaTime);
    }

    private void ChangeLeftOrRight()
    {
        if (leftOrRight)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            leftOrRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            leftOrRight = true;
        }
    }


    // Sent when another object enters a trigger collider attached to this object
    void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.name);
        // If the enemy collide with a Player, hurt him and change direction
        if (collision.transform.gameObject.layer == playerMask)
        {
            Debug.Log("Collision with player");
            //Eventually damage the player
            ChangeLeftOrRight();
        }
        else if (collision.transform.gameObject.layer != groundCheckMask)
        {
            Debug.Log("Collision with something not player:" + collision.collider.name);
            ChangeLeftOrRight();
        }
        else if (flyingEnemy && collision.transform.gameObject.layer == groundCheckMask)
        {
            print("Ciao");
            upOrDown = !upOrDown;
        }
        else
        {
            Debug.Log("Collision with Ground");
        }
    }
}
