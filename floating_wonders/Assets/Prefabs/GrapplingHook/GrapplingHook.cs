using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    DistanceJoint2D joint;
    [SerializeField] private float maxDistance = 100f;
    private RaycastHit2D hit;

    private void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    public void Throw(Transform firePoint, Vector2 direction)
    {
        hit = Physics2D.Raycast(firePoint.position, direction, maxDistance);
        if (hit.collider != null)
        { 
            print("colpito");
            joint.enabled = true;
            joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
            joint.distance = Vector2.Distance(firePoint.position, hit.point);
        }
    }
}
