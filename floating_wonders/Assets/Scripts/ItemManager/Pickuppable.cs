﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


//un gameobject con questo componente è un oggetto che si può raccogliere e utilizzare
public class Pickuppable : NetworkBehaviour
{
    [SerializeField] private ItemType type = ItemType.nullItem;
    [SerializeField] private int respawnTime = 5;
    [SerializeField] private bool canRespawn = true;
    public ItemType Type { get { return type; }}

    public void Pickup()
    {
        Cmd_setActive(false);
    }

    [Command]
    public void Cmd_Respawn()
    {
        if(canRespawn)
            StartCoroutine(WaitCoroutine(5));
    }

    private IEnumerator WaitCoroutine(int secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        Cmd_setActive(true);
    }

    //chiede al server di nascondere l'oggetto
    [Command]
    void Cmd_setActive(bool active)
    {
        Rpc_setActive(active);
    }
    //tutti i client nascondono effettivamente l'oggetto
    [ClientRpc]
    void Rpc_setActive(bool active)
    {
        this.gameObject.GetComponent<Renderer>().enabled = active;
        this.gameObject.GetComponent<Collider2D>().enabled = active;
    }
}
