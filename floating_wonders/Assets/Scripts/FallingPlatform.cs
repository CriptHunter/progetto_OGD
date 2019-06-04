using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FallingPlatform : NetworkBehaviour
{
    private Rigidbody2D rigidBody;
    private LayerMask pLayer;
    [SerializeField] private float fallingDelay;
    private Vector3 startingPosition;
    private bool platformMovingBack;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        pLayer = LayerMask.NameToLayer("Player");
        startingPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (isServer)
        {
            if (platformMovingBack)
                transform.position = Vector2.MoveTowards(transform.position, startingPosition, 20f * Time.deltaTime);

            if (transform.position.y == startingPosition.y)
                platformMovingBack = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == pLayer)
        {
            Invoke("DropPlatform", fallingDelay);
        }
    }

    void DropPlatform()
    {
        rigidBody.isKinematic = false;
        //rigidBody.AddForce(Vector2.down * 100f);
        GetComponent<Collider2D>().isTrigger = true;
        Invoke("GetPlatformBack", 2f);
    }

    void GetPlatformBack()
    {
        rigidBody.velocity = Vector2.zero;
        rigidBody.isKinematic = true;
        GetComponent<Collider2D>().isTrigger = false;
        platformMovingBack = true;
    }

}
