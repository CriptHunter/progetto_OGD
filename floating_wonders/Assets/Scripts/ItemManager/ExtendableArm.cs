using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExtendableArm : NetworkBehaviour
{
    private LayerMask ignoredLayer = ~((1 << 2) | (1 << 9));
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private float maxDistance = 30f;  //massima distanza del braccio
    private RaycastHit2D hit; //oggetto da raccogliere

    public void Throw(Vector2 direction)
    {
        hit = Physics2D.Raycast(firePoint.position, direction, maxDistance, ignoredLayer);
        if (hit.collider != null && hit.transform.GetComponent<Pickuppable>() != null)
        {
            print("raycast");
            GetComponent<ItemManager>().Pickup(hit.collider.gameObject);
        }
    }
}
