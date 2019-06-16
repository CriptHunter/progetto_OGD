using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerWalker : MonoBehaviour
{
    // Start is called before the first frame update
    public float zoom = 1;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * 3.4f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * 3.4f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * 3.4f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * 3.4f * Time.deltaTime);
        }


    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            zoom /= 1.0075f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            zoom *= 1.0075f;
        }
    }
}
