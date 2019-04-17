using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class EnemySimpleMovement : MonoBehaviour
{
    public float speed;
    public float distance;

    private bool movingRight = true;
    private LayerMask playerMask;

    public Transform groundDetection;


    private void Start()
    {
        playerMask = LayerMask.NameToLayer("Player");
    }

    void Update()
    {

        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if (groundInfo.collider == false)
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
        // If the enemy collide with an Enemy, hurt him and change direction
        if (collision.transform.gameObject.layer == playerMask)
        {
            Debug.Log("Collision with player");
            //Eventually damage the player
            ChangeDirection();
        }
        else
        {
            Debug.Log("Collision with something (No player)");
            ChangeDirection();
        }
    }
}
