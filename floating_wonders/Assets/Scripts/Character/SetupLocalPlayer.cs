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
        var itemManager = this.GetComponent<ItemManager>();
        var renderer = GetComponent<Renderer>();

        if (gh != null && ea != null)
        {
            if (isServer && isLocalPlayer)
            {
                gh.enabled = false;
                ea.enabled = true;
                itemManager.uniqueItem = ItemType.extendableArm;
            }
            else if (!isServer && isLocalPlayer)
            {
                gh.enabled = true;
                ea.enabled = false;
                itemManager.uniqueItem = ItemType.grapplingHook;
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
