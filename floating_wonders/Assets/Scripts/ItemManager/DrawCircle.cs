<<<<<<< Updated upstream:floating_wonders/Assets/Scripts/DrawCircle.cs
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    [Range(0, 50)] [SerializeField] private int segments = 50;
    [Range(0, 30)] [SerializeField] private float xradius = 5;
    [Range(0, 30)] [SerializeField] private float yradius = 5;
    //Centro del cerchio
    [SerializeField] private Transform center;

    private LineRenderer line;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.transform.position = center.position;
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
    }

    public void ShowCircle()
    {
        float x;
        float y;
        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;
            if (line != null)
            {
                line.SetPosition(i, new Vector3(x, y, 0));
            }
            angle += (360f / segments);
        }
        line.enabled = true;
    }

    public void HideCircle()
    {
        line.enabled = false;
    }
}

=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    [Range(0, 50)] [SerializeField] private int segments = 50;
    [Range(0, 30)] [SerializeField] private float xradius = 5;
    [Range(0, 30)] [SerializeField] private float yradius = 5;
    //Centro del cerchio
    [SerializeField] private Transform center;

    private LineRenderer line;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.transform.position = center.position;
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
    }

    public void ShowCircle()
    {
        float x;
        float y;
        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;
            if (line != null)
            {
                line.SetPosition(i, new Vector3(x, y, 0));
            }
            angle += (360f / segments);
        }
        line.enabled = true;
    }

    public void HideCircle()
    {
        line.enabled = false;
    }

    public void SetLineColor(Color c)
    {
        line.startColor = c;
        line.endColor = c;
    }
}

>>>>>>> Stashed changes:floating_wonders/Assets/Scripts/ItemManager/DrawCircle.cs
