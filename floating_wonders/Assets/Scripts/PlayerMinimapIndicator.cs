using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMinimapIndicator : MonoBehaviour
{
    private RectTransform rt;
    private GameObject target = null;
    private Image img;

    private float xmin;
    private float ymin;
    private float xmax;
    private float ymax;
    private Vector2 parentSize;
    private void Start()
    {
        img = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    public void SetTarget(GameObject target)
    {
        this.target = target;
        //print("map actual size is " + transform.parent.GetComponent<RectTransform>().ActualSize());
        parentSize = transform.parent.GetComponent<RectTransform>().ActualSize();
    }

    public void SetBounds(float xmin, float ymin, float xmax, float ymax)
    {
        this.xmin = xmin;
        this.ymin = ymin;
        this.xmax = xmax;
        this.ymax = ymax;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (!img.enabled)
            {
                img.enabled = true;
            }
            float posx = Mathf.InverseLerp(xmin, xmax, target.transform.position.x);
            float posy = Mathf.InverseLerp(ymin, ymax, target.transform.position.y);
            rt.localPosition = new Vector2(-parentSize.x,0) + new Vector2(posx*parentSize.x,posy*parentSize.y);
        }
        else
        {
            if (img.enabled)
            {
                img.enabled = false;
            }
        }
    }
}
