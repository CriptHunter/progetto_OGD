using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMinimapIndicator : MonoBehaviour
{
    private RectTransform rt;
    private GameObject target = null;
    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            rt.localPosition = Vector2.zero;
        }
    }
}
