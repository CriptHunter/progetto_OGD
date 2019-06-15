using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


//un gameobject con questo componente è un oggetto che si può raccogliere e utilizzare
public class Pickuppable : NetworkBehaviour
{
    [SerializeField] private ItemType type = ItemType.nullItem;
    [SerializeField] private int respawnTime = 5; //tempo in secondi prima del respawn
    [SerializeField] private bool canRespawn = true; //se può respawnare
    [SerializeField] public bool collectible; //se è un collezionabile e non un oggetto normale
    public ItemType Type { get { return type; }}

    public void Pickup()
    {
        //se è un collezionabile, aggiorno l'HUD
        if (collectible)
            GameManager.Instance.GrabCollectible(type);
        if (canRespawn)
            Cmd_Respawn();
        Rpc_setActive(false);
    }

    private void Start()
    {
        //i collezionabili non rinascono
        if (collectible)
            canRespawn = false;
    }

    [Command]
    public void Cmd_Respawn()
    {
        if(canRespawn)
            StartCoroutine(WaitCoroutine(respawnTime));
    }

    private IEnumerator WaitCoroutine(int secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        Rpc_setActive(true);
    }

    //tutti i client nascondono effettivamente l'oggetto
    [ClientRpc]
    void Rpc_setActive(bool active)
    {
        //alcuni pickuppable hanno lo sprite renderer, altri spine skeleton
        if(this.gameObject.GetComponent<SpriteRenderer>() != null)
            this.gameObject.GetComponent<Renderer>().enabled = active;
        if (this.gameObject.GetComponentInChildren<SkeletonAnimation>() != null)
            this.gameObject.GetComponentInChildren<SkeletonAnimation>().skeleton.SetColor(new Color(0, 0, 0, 0));
        this.gameObject.GetComponent<Collider2D>().enabled = active;
        
    }
}
