using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExtendableArm : NetworkBehaviour
{
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private float maxDistance = 30f;  //massima distanza del braccio
    private RaycastHit2D hit; //oggetto da raccogliere

    public void Throw(Vector2 direction)
    {
        hit = Physics2D.Raycast(firePoint.position, direction, maxDistance);
        if (hit.collider != null && hit.transform.GetComponent<Pickuppable>() != null)
        {
            print("raycast");
            GetComponent<PickUpThrow>().Pickup(hit.collider.gameObject);
        }
    }
}
