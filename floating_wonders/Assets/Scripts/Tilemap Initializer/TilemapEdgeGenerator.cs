using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilemapEdgeGenerator : MonoBehaviour
{
    public BoxCollider2D horizontalFreespace;
    public BoxCollider2D verticalFreespace;
    public BoxCollider2D edgeChecker;
    public GameObject edgeGameObject;
    public Transform edgePosition;
    public LayerMask solid;
    
    private float delay = 2;

    // call this when you are ready to test if there is an edge here
    public void GenerateEdge()
    {
        if (horizontalFreespace!=null && verticalFreespace!=null && edgeChecker != null)
        {
            BoxCollider2D box;

            box = horizontalFreespace;
            RaycastHit2D hitHor = Physics2D.BoxCast(box.bounds.center, box.bounds.size, 0, Vector2.one, 0f, solid);
            box = verticalFreespace;
            RaycastHit2D hitVer = Physics2D.BoxCast(box.bounds.center, box.bounds.size, 0, Vector2.one, 0f, solid);
            box = edgeChecker;
            RaycastHit2D hitEdge = Physics2D.BoxCast(box.bounds.center, box.bounds.size, 0, Vector2.one, 0f, solid);


            bool result = (hitHor.collider == null && hitVer.collider==null && hitEdge.collider!=null);

            if (result && delay<=0)
            {
                var go = Instantiate(edgeGameObject);
                go.transform.position = edgePosition.transform.position;
            }
        }
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        delay -= 1; // add a delay or unity's system will not detect raycasts
        if (delay <= 0)
        {
            GenerateEdge();
        }
    }
}
