using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyLookingForYou : MonoBehaviour
{
    private PlayerController playerController;
    private float startingSpeed;

    [SerializeField]
    private float speed;
    public float groundRayDistance;
    public float sightDistance;
    public float speedAfterSeen;

    private bool movingRight = true;

    public Transform groundDetection;
    private GameObject playerPosition;

    private RaycastHit2D groundInfo;
    private RaycastHit2D playerAtLeft;
    private RaycastHit2D playerAtRight;

    private LayerMask mask;

    [SerializeField]
    private bool untilEndPlatform;

    [SerializeField]
    private float moveDistance;


    private void InstantiateRay()
    {
        groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, groundRayDistance);
        playerAtLeft = Physics2D.Raycast(transform.position, Vector2.left, sightDistance, mask);
        playerAtRight = Physics2D.Raycast(transform.position, Vector2.right, sightDistance, mask);


        Debug.DrawRay(transform.position, Vector2.right * sightDistance, Color.green);
        Debug.DrawRay(transform.position, Vector2.left * sightDistance, Color.green);

    }

    private void Patrol()
    {
        if (untilEndPlatform)
            //Move and keep moving enemy
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        /*else
            playerController.Move(moveDistance, false);*/
    }

    private void AvoideFall()
    {
        // If groundInfo.collider == false means that the enemy is about to fall from the platform
        if (groundInfo.collider == false)
        {
            Debug.Log("FloorEnded");
            // If enemy is moving right, change direction
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
        // If the enemy is not about to fall
        else
        {
            Debug.Log("FloorOk");
            LookForPlayer();
        }
    }

    private void LookForPlayer()
    {
        
        // If right, the enemy is moving right, if == false left
        if (playerAtRight.collider && movingRight)
        {
            Debug.Log("Player At RIGHT: " + playerAtRight.collider.name);
            speed = speedAfterSeen;
        }
        else if (playerAtLeft.collider && !movingRight)
        {
            Debug.Log("Player At LEFT: " + playerAtLeft.collider.name);
            speed = speedAfterSeen;
        }
        else
        {
            Debug.Log("Blind");
            speed = startingSpeed;
        }
    }

    void Start()
    {
        //playerController = GetComponent<PlayerController>();
        startingSpeed = speed;
    }


    void Update() 
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player");
        mask = LayerMask.GetMask("Player");
        //Instantiate ray to not fall and to see player
        InstantiateRay();

        //The enemy starts moving
        Patrol();

        AvoideFall();

    }

}
