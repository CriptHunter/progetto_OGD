using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookingForYou : MonoBehaviour
{
    //private PlayerController pc;

    public static float speed;
    public float groundRayDistance;
    public float sightDistance;
    public float speedAfterSeen;

    private bool movingRight = true;

    public Transform groundDetection;


    private RaycastHit2D groundInfo;
    private RaycastHit2D playerAtLeft;
    private RaycastHit2D playerAtRight;


    private void InstantiateRay()
    {
        groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, groundRayDistance);
        playerAtLeft = Physics2D.Raycast(transform.position, Vector2.left, sightDistance);
        playerAtRight = Physics2D.Raycast(transform.position, Vector2.right, sightDistance);

        Debug.DrawRay(transform.position, Vector2.right * sightDistance, Color.green);
        Debug.DrawRay(transform.position, Vector2.left * sightDistance, Color.green);
    }

    private void LookForPlayer(bool right)
    {
        // If right the enemy is moving right, if == false left
        if (playerAtRight.collider && right)
        {
            Debug.Log("Player At RIGHT: " + playerAtRight.collider.name);
            speed = speedAfterSeen;
        }
        else if (playerAtLeft.collider && !right)
        {
            Debug.Log("Player At LEFT: " + playerAtLeft.collider.name);
            speed = speedAfterSeen;
        }
        else
        {
            Debug.Log("Cieco");
            speed = 2;
        }
    }

    void Update()
    {
        //Move and keep moving enemy
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        //Instantiate ray to not fall and to see player
        InstantiateRay();


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
            if (movingRight)
            {
                LookForPlayer(true);
            }
            else
            {
                LookForPlayer(false);
            }
        }    
    }
}
