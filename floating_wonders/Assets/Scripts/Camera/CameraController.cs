using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(OrthoCameraBehaviour))]
public class CameraController : MonoBehaviour
{
    public GameObject target;
    private OrthoCameraBehaviour ocb;

    private void Start()
    {
        ocb = GetComponent<OrthoCameraBehaviour>();
    }

    void LateUpdate()
    {
        if (target!=null)
        {
            ocb.SetFocus(target.transform.position);
        }
    }
}
