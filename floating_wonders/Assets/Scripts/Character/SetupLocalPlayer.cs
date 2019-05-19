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
        if (isServer && isLocalPlayer)
        {
            this.GetComponent<GrapplingHook>().enabled = false;
            this.GetComponent<ExtendableArm>().enabled = true;
            this.GetComponent<PickUpThrow>().uniqueItem = ItemType.extendableArm;
            GetComponent<Renderer>().material.color = Color.magenta;
        }

        else if (!isServer)
        {
            if (isLocalPlayer)
            {
                this.GetComponent<GrapplingHook>().enabled = true;
                this.GetComponent<ExtendableArm>().enabled = false;
                this.GetComponent<PickUpThrow>().uniqueItem = ItemType.grapplingHook;
            }
            else
                GetComponent<Renderer>().material.color = Color.magenta;
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
