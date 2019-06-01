using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundInitializer : MonoBehaviour
{
    public List<GameObject> clouds;
    public GameObject bottomlayer;
    public int bottomLayerCount;
    // Start is called before the first frame update
    void Start()
    {
        var backcolor = Camera.main.backgroundColor;
        for (int i = 0; i < bottomLayerCount; i++)
        {
            var go = Instantiate(bottomlayer);
            float progress= (float)i / Mathf.Max(bottomLayerCount - 1, 1);
            go.transform.position = new Vector3(0, 0, Mathf.Lerp(10000,100,progress));
            go.transform.localScale = Vector3.one * Mathf.Lerp(1000, 10, progress);
            var renderer = go.GetComponent<SpriteRenderer>();
            renderer.color = Util.FlatColor(Color.Lerp(backcolor, Color.white, progress));
            renderer.sortingOrder = -(int)go.transform.position.z;

        }
    }
}
