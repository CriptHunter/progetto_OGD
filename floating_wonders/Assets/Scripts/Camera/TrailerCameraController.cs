using System.Collections;
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