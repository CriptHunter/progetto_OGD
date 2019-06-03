using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class KeyUnlockDoor : NetworkBehaviour
{
    [SerializeField] private int requestedKeys;
    private bool open;
    [SerializeField] private Text keysToUnlock;


    private void Update()
    {
        if (isServer)
            RpcSetKeys(GameManager.Instance.GetKeys());
    }

    public void CheckDoor()
    {
        if (isServer)
        { 
            print("OpeningDoor");
            if (GameManager.Instance.GetKeys() >= requestedKeys && !open)
            {
                transform.position += new Vector3(0, 4, 0);
                open = true;
                RpcDeactivate();
        }
        else
            print("You don't have enough keys");
    }
    }


    [ClientRpc] private void RpcDeactivate()
    {
        keysToUnlock.gameObject.SetActive(false);
        this.GetComponentInChildren<PadKeyUnlockDoor>().gameObject.SetActive(false);
    }

    [ClientRpc] private void RpcSetKeys(int keys)
    {
        keysToUnlock.text = keys + " / " + requestedKeys;
    }
}
