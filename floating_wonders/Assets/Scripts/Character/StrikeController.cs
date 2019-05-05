using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class StrikeController : MonoBehaviour
{
    private new CircleCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    public bool PerformStrike()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(collider.bounds.center, collider.radius, Vector2.right, 0.0f);
        print("Striking: " + hits.Length);

        for (int i = 0; i < hits.Length; i++)
        {
            print("hit: " + hits[i].collider.gameObject.name);
            Strikeable strikeable = hits[i].collider.gameObject.GetComponent<Strikeable>();
            if (strikeable != null)
            {
                strikeable.Strike();
            }
        }
        return false;
    }
}
