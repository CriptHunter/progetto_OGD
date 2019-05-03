using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            AnotherCharacterInput anotherCaracterInput = GetComponent<AnotherCharacterInput>();
            if (anotherCaracterInput != null)
            {
                anotherCaracterInput.enabled = false;
            }
        }
    }
}
