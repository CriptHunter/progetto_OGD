using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundInitializer : MonoBehaviour
{
    public List<GameObject> clouds;
    public GameObject bottomlayer;
    public int bottomLayerCount;


    private Bounds bounds;
    // Start is called before the first frame update
    void Start()
    {
        var backcolor = Camera.main.backgroundColor;
        var minz = 10;
        var maxz = 1000;

        foreach (Tilemap tm in gameObject.GetComponentsInChildren<Tilemap>())
        {
            //Bounds mybound = new Bounds (Vector3.zero, new Vector3(tm.cellBounds.size.x * tm.cellSize.x, tm.cellBounds.size.y * tm.cellSize.y));
            bounds.Encapsulate(tm.localBounds);
        }

        //bounds.center = Vector3.zero;

        Vector3 p1, p2, p3, p4, p5, p6, p7, p8, p5d, p6d, p7d, p8d;
        p1 = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, 0);

        p2 = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, 0);

        p3 = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, 0);

        p4 = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, 0);

        Camera.main.transform.position = new Vector3(p1.x, p1.y, Camera.main.transform.position.z);
        p5 = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, minz));
        p5d = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, maxz));

        Camera.main.transform.position = new Vector3(p2.x, p2.y, Camera.main.transform.position.z);
        p6 = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, minz));
        p6d = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, maxz));


        Camera.main.transform.position = new Vector3(p3.x, p3.y, Camera.main.transform.position.z);
        p7 = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, minz));
        p7d = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, maxz));

        Camera.main.transform.position = new Vector3(p4.x, p4.y, Camera.main.transform.position.z);
        p8 = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, minz));
        p8d = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, maxz));

        for (int i=0; i<99; i++)
        {
            var go = Instantiate(Util.ChooseFrom(clouds));
            var zlayer = Util.Random(1);

            Vector3 botleft, topright;
            botleft = Vector3.LerpUnclamped(p8, p8d, zlayer);
            topright = Vector3.LerpUnclamped(p6, p6d, zlayer);
            go.transform.position = Util.RandomRange(botleft, topright);
            go.transform.localScale = Vector3.one * Mathf.Lerp(8,96,zlayer);

            var renderer = go.GetComponent<SpriteRenderer>();
            renderer.color = Util.FlatColor(Color.Lerp(Color.white, backcolor, zlayer));
            renderer.sortingOrder = -(int)go.transform.position.z;
        }

        for (int i = 1; i < bottomLayerCount; i++)
        {
            var go = Instantiate(bottomlayer);
            float progress = (float)i / Mathf.Max(bottomLayerCount, 1);
            go.transform.position = new Vector3(Util.Mean(p8.x,p7.x), Mathf.Lerp(Mathf.Lerp(p8.y, p8d.y, 0.2f), Mathf.Lerp(p8.y,p8d.y,0.7f),progress), Mathf.Lerp(Mathf.Lerp(minz,maxz,0.05f), maxz, progress));
            go.transform.localScale = Vector3.one * Mathf.Lerp(20, 150, progress);
            var renderer = go.GetComponent<SpriteRenderer>();
            renderer.color = Util.FlatColor(Color.Lerp(Color.white, backcolor, progress));
            renderer.sortingOrder = -(int)go.transform.position.z;

            var go1= Instantiate(bottomlayer);
            go1.transform.position = go.transform.position + new Vector3(renderer.bounds.extents.x * 2, 0, 0);
            go1.transform.localScale = go.transform.localScale;
            var renderer1 = go1.GetComponent<SpriteRenderer>();
            renderer1.color = renderer.color;
            renderer1.sortingOrder = renderer.sortingOrder;

            var go2 = Instantiate(bottomlayer);
            go2.transform.position = go.transform.position - new Vector3(renderer.bounds.extents.x * 2, 0, 0);
            go2.transform.localScale = go.transform.localScale;
            var renderer2 = go2.GetComponent<SpriteRenderer>();
            renderer2.color = renderer.color;
            renderer2.sortingOrder = renderer.sortingOrder;

            float xshift = Util.RandomRange(-renderer.bounds.extents.x, renderer.bounds.extents.x);
            //print("shifting by "+xshift);
            go.transform.position += Vector3.right * xshift;
            go1.transform.position += Vector3.right * xshift;
            go2.transform.position += Vector3.right * xshift;

        }

        /*
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = p1;

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = p2;

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = p3;

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = p4;

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = p5;

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = p6;

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = p7;

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = p8;
        */
    }
}
