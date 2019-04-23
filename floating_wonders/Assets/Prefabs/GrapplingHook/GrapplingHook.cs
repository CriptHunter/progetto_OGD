using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("ho il rampino lmao");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Throw(Rigidbody2D playerRb, Transform firePoint, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction);
;        if (hit.collider != null)
        {
            print(direction);
            playerRb.velocity = direction * 50;
        }

    }
}
