using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookingForYou : MonoBehaviour
{
    private PlayerController pc;

    public static float speed;
    public float groundRayDistance;
    public float sightDistance;
    public float speedAfterSeen;

    private bool movingRight = true;

    public Transform groundDetection;

    //private float speed2;

    private void Start()
    {
        pc = GetComponent<PlayerController>();
        
    }

    void Update()
    {

        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, groundRayDistance);


        RaycastHit2D playerAtLeft = Physics2D.Raycast(transform.position, Vector2.left, sightDistance);
        RaycastHit2D playerAtRight = Physics2D.Raycast(transform.position, Vector2.right, sightDistance);

        Debug.DrawRay(transform.position, Vector2.right * sightDistance, Color.green);
        Debug.DrawRay(transform.position, Vector2.left * sightDistance, Color.green);
        

        if (groundInfo.collider == false)
        {
            Debug.Log("FloorEnded");
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
        else
        {
            Debug.Log("FloorOk");
            if (movingRight)
            {
                if (playerAtRight.collider)
                {
                    Debug.Log("Player At RIGHT: " + playerAtRight.collider.name);
                    speed = speedAfterSeen;
                }
                else
                {
                    Debug.Log("Cieco");
                    speed = 2;
                }
            }
            else
            {
                if (playerAtLeft.collider)
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

        }
            
    }
}
