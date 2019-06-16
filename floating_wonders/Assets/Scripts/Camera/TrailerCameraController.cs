<<<<<<< Updated upstream
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(OrthoCameraBehaviour))]
public class TrailerCameraController : MonoBehaviour
{
    public GameObject target;
    private OrthoCameraBehaviour ocb;

    private void Start()
    {
        ocb = GetComponent<OrthoCameraBehaviour>();
        if (target != null)
        {
            ocb.SetFocus(target.transform.position,true);
            ocb.SetZoom(target.GetComponent<TrailerWalker>().zoom,true);
        }
    }

    void LateUpdate()
    {
        if (target!=null)
        {
            ocb.SetFocus(target.transform.position);
            ocb.SetZoom(target.GetComponent<TrailerWalker>().zoom);
        }
    }
}
=======
<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(OrthoCameraBehaviour))]
public class TrailerCameraController : MonoBehaviour
{
    public GameObject target;
    private OrthoCameraBehaviour ocb;

    private void Start()
    {
        ocb = GetComponent<OrthoCameraBehaviour>();
        if (target != null)
        {
            ocb.SetFocus(target.transform.position,true);
            ocb.SetZoom(target.GetComponent<TrailerWalker>().zoom,true);
        }
    }

    void LateUpdate()
    {
        if (target!=null)
        {
            ocb.SetFocus(target.transform.position);
            ocb.SetZoom(target.GetComponent<TrailerWalker>().zoom);
        }
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(OrthoCameraBehaviour))]
public class TrailerCameraController : MonoBehaviour
{
    public GameObject target;
    private OrthoCameraBehaviour ocb;

    private void Start()
    {
        ocb = GetComponent<OrthoCameraBehaviour>();
        if (target != null)
        {
            ocb.SetFocus(target.transform.position,true);
            ocb.SetZoom(target.GetComponent<TrailerWalker>().zoom,true);
        }
    }

    void LateUpdate()
    {
        if (target!=null)
        {
            ocb.SetFocus(target.transform.position);
            ocb.SetZoom(target.GetComponent<TrailerWalker>().zoom);
        }
    }
}
>>>>>>> c964f8996061b22d07a365be3f810aeab30371cd
>>>>>>> Stashed changes
