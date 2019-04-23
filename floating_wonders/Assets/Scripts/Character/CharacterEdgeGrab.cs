using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEdgeGrab : MonoBehaviour
{
    public LayerMask target;

    private new BoxCollider2D collider;
    private GameObject edge;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = collider.SelfCast(target);
        if (hit.collider!=null)
        {
            edge = hit.collider.gameObject;
        }
        else
        {
            edge = null;
        }

        /*if (edge != null)
        {
            Destroy(edge);
        }*/
    }

    public GameObject GetEdge()
    {
        return edge;
    }
}
