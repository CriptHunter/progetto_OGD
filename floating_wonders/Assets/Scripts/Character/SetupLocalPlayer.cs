using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Il server ha il braccio steampunk, il client il rampino
        var gh = GetComponent<GrapplingHook>();
        var ea = this.GetComponent<ExtendableArm>();
        var pt = this.GetComponent<PickUpThrow>();
        var renderer = GetComponent<Renderer>();

        if (isServer && isLocalPlayer)
        {
            if (gh != null)
            {
                gh.enabled = false;
            }
            if (ea != null)
            {
                ea.enabled = true;
            }
            if (pt != null)
            {
                pt.uniqueItem = ItemType.extendableArm;
            }
            if (renderer != null)
            {
                renderer.material.color = Color.magenta;
            }
        }

        else if (!isServer)
        {
            if (isLocalPlayer)
            {
                if (gh != null)
                {
                    gh.enabled = true;
                }
                if (ea != null)
                {
                    ea.enabled = false;
                }
                if (pt != null)
                {
                    pt.uniqueItem = ItemType.grapplingHook;
                }
            }
            else
            {
                if (renderer != null)
                {
                    renderer.material.color = Color.magenta;
                }
            }
        }

        if (!isLocalPlayer)
        {
            AnotherCharacterInput anotherCaracterInput = GetComponent<AnotherCharacterInput>();
            if (anotherCaracterInput != null)
            {
                anotherCaracterInput.enabled = false;
            }
            AnotherCharacterController anotherCaracterController = GetComponent<AnotherCharacterController>();
            if (anotherCaracterController != null)
            {
                anotherCaracterController.enabled = false;
            }
        }
        else
        {
            CameraController controller = Camera.main.GetComponent<CameraController>();
            if (controller != null)
            {
                controller.target = gameObject;
            }
        }
    }
}
