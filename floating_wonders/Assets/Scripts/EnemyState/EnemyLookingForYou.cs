﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyLookingForYou : NetworkBehaviour
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

    private RaycastHit2D groundInfo;
    private RaycastHit2D playerAtLeft;
    private RaycastHit2D playerAtRight;

    private LayerMask playerMask;
    private LayerMask enemyMask;
    private LayerMask groundMask;

    //[SerializeField]
    private bool untilEndPlatform = true;

    /*[SerializeField]
    private float moveDistance;*/

    void Start()
    {
        playerMask = LayerMask.NameToLayer("Player");
        // Mask to not hit himself
        enemyMask = ~(1 << 10);
        startingSpeed = speed;
        groundMask = LayerMask.NameToLayer("Ground");
    }


    void Update()
    {
        //Instantiate ray to not fall and to see player
        InstantiateRay();

        //The enemy starts moving
        Patrol();

        AvoideFall();

    }

    private void InstantiateRay()
    {
        groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, groundRayDistance);
        playerAtLeft = Physics2D.Raycast(transform.position, Vector2.left, sightDistance, enemyMask);
        playerAtRight = Physics2D.Raycast(transform.position, Vector2.right, sightDistance, enemyMask);


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
            ChangeDirection();
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
        if (playerAtRight.collider != null && playerAtRight.transform.gameObject.layer == playerMask && movingRight)
        {
            Debug.Log("Player At RIGHT: " + playerAtRight.collider.name);
            speed = speedAfterSeen;
        }
        else if (playerAtLeft.collider != null && playerAtLeft.transform.gameObject.layer == playerMask && !movingRight)
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

    private void ChangeDirection()
    {
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
        else if (collision.transform.gameObject.layer != groundMask)
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
