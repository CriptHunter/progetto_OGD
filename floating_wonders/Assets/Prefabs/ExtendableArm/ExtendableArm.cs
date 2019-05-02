using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendableArm : MonoBehaviour
{
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private float maxDistance = 10f;  //massima distanza del braccio
    [SerializeField] private float step = 0.2f; //di quanto si accorcia il braccio ogni volta
    private RaycastHit2D hit; //oggetto da trascinare
    private DistanceJoint2D joint;
    private bool anchored; //true se il personaggio è attaccato con il braccio

    private void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    public void Update()
    {
        if (anchored)
        {
            if (joint.distance > 0)
                joint.distance = joint.distance - step;
            else
            {
                anchored = false;
            }
        }
    }

    public void Throw(Vector2 direction)
    {
        hit = Physics2D.Raycast(firePoint.position, direction, maxDistance);
        if (hit.collider != null)
        {
            anchored = true;
            joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
            print("uso braccio");
        }
    }
}
