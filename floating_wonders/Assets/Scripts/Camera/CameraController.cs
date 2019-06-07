using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(OrthoCameraBehaviour))]
public class CameraController : MonoBehaviour
{
    public GameObject target;
    private OrthoCameraBehaviour ocb;
    private AnotherCharacterController acc;
    private bool notSureIfDoesntHaveAcc=true;

    private float zoomProgress = 0;
    private float zoomTime = 10f;

    private void Start()
    {
        ocb = GetComponent<OrthoCameraBehaviour>();
        if (target != null)
        {
            acc = target.GetComponent<AnotherCharacterController>();
            if (acc == null)
            {
                notSureIfDoesntHaveAcc = false;
            }
        }
    }

    void LateUpdate()
    {
        if (target!=null)
        {
            Vector2 previousFocus = ocb.GetFocus();
            if (acc != null)
            {
                if (zoomProgress >= zoomTime)
                {
                    ocb.SetFocus(target.transform.position);
                }
                else
                {
                    ocb.SetFocusX(target.transform.position.x + 1.4f * (int)acc.GetVerse() * (acc.IsClimbing() ? -1 : 1));
                    ocb.SetFocusY(target.transform.position.y);
                }
            }
            else
            {
                if (notSureIfDoesntHaveAcc)
                {
                    acc = target.GetComponent<AnotherCharacterController>();
                    if (acc == null)
                    {
                        notSureIfDoesntHaveAcc = false;
                    }
                }

                ocb.SetFocus(target.transform.position);
            }
            Vector2 sequentFocus = ocb.GetFocus();

            if (previousFocus == sequentFocus)
            {
                zoomProgress += Time.deltaTime;
                if (zoomProgress >= zoomTime)
                {
                    ocb.SetFocus(target.transform.position);
                    ocb.SetZoom(0.5f);
                }
            }
            else
            {
                ocb.SetZoom(1);
                zoomProgress = 0;
            }

        }
    }
}
