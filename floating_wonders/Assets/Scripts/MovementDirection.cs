using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDirection : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Vector2 directionVector;
    [SerializeField] private float distance;
    [SerializeField] private float speed;

    private Vector3 startingPos;
    bool moveRight;
    private Vector3 directionVector3;

    private void Start()
    {
        //moveRight = false;
        
        startingPos = transform.position;
    }

    void Update()
    {
        directionVector3 = new Vector3(directionVector.x, directionVector.y, 0);
        if (transform.position == startingPos + directionVector3 * distance)
            moveRight = false;
        if (transform.position == startingPos - directionVector3 * distance)
            moveRight = true;

        if (moveRight)
            transform.position = Vector2.MoveTowards(transform.position, startingPos + directionVector3 * distance, speed * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(transform.position, startingPos - directionVector3 * distance, speed * Time.deltaTime);
    }
}
