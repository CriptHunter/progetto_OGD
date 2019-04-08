using UnityEngine;
using UnityEngine.Networking;

public class PickUpThrow : NetworkBehaviour
{
    private bool pickUpAllowed; //true quando il giocatore è in contatto con un oggetto che si può raccogliere
    private GameObject collidedObject; //con quale gameobject il giocatore è entrato in contatto
    private int pickedUpItem = (int)EnumCollection.itemsEnum.nullItem; //quale oggetto è stato raccolto, -1 --> nessun oggetto
    [SerializeField] private Transform firePoint = null; // da quale punto usa l'oggetto
    //i veri oggetti utilizzabili
    [SerializeField] private GameObject BombRBPrefab = null;

    private void Update()
    {
        //se posso raccogliere qualcosa e premo E --> raccoglie oggetto
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E) && isLocalPlayer)
        {
            pickedUpItem = (int)collidedObject.GetComponent<Pickuppable>().Type;
            Cmd_Pickup(collidedObject);
        }

        //se o un oggetto in mano e premo R --> usa oggetto
        else if (pickedUpItem >= 0 && Input.GetKeyDown(KeyCode.R) && isLocalPlayer)
        {
            useItem();
        }
    }

    //quando il giocatore collide con un oggetto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Pickuppable>() != null && isLocalPlayer)
        {
            pickUpAllowed = true;
            collidedObject = collision.gameObject;
        }
    }

    //quando il giocatore esce dall'area di collisione
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Pickuppable>() != null && isLocalPlayer)
        {
            pickUpAllowed = false;
            collidedObject = collision.gameObject;
        }
    }

    //usa l'oggetto equipaggiato
    private void useItem()
    {
        switch (pickedUpItem)
        {
            case (int)EnumCollection.itemsEnum.bomb:
                print("uso bomba");
                Cmd_SpawnBomb();
                break;
            case (int)EnumCollection.itemsEnum.redItem:
                print("uso oggetto 1");
                break;
            case (int)EnumCollection.itemsEnum.blueItem:
                print("uso oggetto 2");
                break;        
        }
    }



    //chiede al server di distruggere l'oggetto
    [Command]
    void Cmd_Pickup(GameObject obj)
    {
        Destroy(obj);
    }

    //Ad un command non si può passare un gameobject / prefab da spawnare, quindi ho fatto un metodo specifico per la bomba
    //un'alternativa brutta è fare un Resource.load() del prefab
    [Command]
    void Cmd_SpawnBomb()
    {
        GameObject obj = (GameObject)Instantiate(BombRBPrefab, firePoint.position, firePoint.rotation);
        NetworkServer.Spawn(obj);
    }

}
