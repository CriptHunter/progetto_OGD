using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlatformMovement : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Vector2 directionVector;
    [SerializeField] private float distance;
    [SerializeField] private float speed;

    private GameObject emptyContainer;
    private Vector3 startingPos;
    bool moveDirection;
    private Vector3 directionVector3;

    private void Start()
    {
        startingPos = transform.position;
        emptyContainer = new GameObject();
    }

    void Update()
    {
        if (!isServer)
            return;
        directionVector3 = new Vector3(directionVector.x, directionVector.y, 0);
        if (transform.position == startingPos + directionVector3 * distance)
            moveDirection = false;
        if (transform.position == startingPos - directionVector3 * distance)
            moveDirection = true;

        if (moveDirection)
            transform.position = Vector2.MoveTowards(transform.position, startingPos + directionVector3 * distance, speed * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(transform.position, startingPos - directionVector3 * distance, speed * Time.deltaTime);
    }

    

}
