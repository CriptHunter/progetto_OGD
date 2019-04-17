using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PickUpThrow : NetworkBehaviour
{
    private bool pickUpAllowed; //true quando il giocatore è in contatto con un oggetto che si può raccogliere
    private GameObject collidedObject; //con quale gameobject il giocatore è entrato in contatto
    private int pickedUpItemType = (int)EnumCollection.itemsEnum.nullItem; //quale oggetto è stato raccolto, -1 --> nessun oggetto
    private GameObject pickedUpItem = null;
    private Vector2 shootDirection;
    [SerializeField] private Transform firePoint = null; // da quale punto usa l'oggetto
    //i veri oggetti utilizzabili
    [SerializeField] private GameObject BombRBPrefab = null;

    private void Update()
    {
        //se posso raccogliere qualcosa e premo E --> raccoglie oggetto
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E) && isLocalPlayer)
        {
            pickedUpItemType = (int)collidedObject.GetComponent<Pickuppable>().Type;
            pickedUpItem = collidedObject;
            pickedUpItem.GetComponent<Pickuppable>().Pickup();
        }

        //se o un oggetto in mano e premo R --> entra in fase di mira
        else if (pickedUpItemType >= 0 && Input.GetKeyDown(KeyCode.R) && isLocalPlayer)
            shootDirection = getAimDirection();

        else if (pickedUpItemType >= 0 && Input.GetKeyUp(KeyCode.R) && isLocalPlayer)
            useItem();

    }

    //quando il giocatore collide con un oggetto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Pickuppable>() != null && isLocalPlayer && pickedUpItemType == (int)EnumCollection.itemsEnum.nullItem)
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
        switch (pickedUpItemType)
        {
            case (int)EnumCollection.itemsEnum.bomb:
                print("uso bomba");
                Cmd_ThrowBomb();
                pickedUpItem.GetComponent<Pickuppable>().Respawn(5);
                break;
            case (int)EnumCollection.itemsEnum.redItem:
                print("uso oggetto 1");
                pickedUpItem.GetComponent<Pickuppable>().Respawn(3);
                break;
            case (int)EnumCollection.itemsEnum.blueItem:
                print("uso oggetto 2");
                pickedUpItem.GetComponent<Pickuppable>().Respawn(3);
                break;        
        }
        pickedUpItemType = (int)EnumCollection.itemsEnum.nullItem;
    }

    //restituisce un vettore 2D che va dal fire point al puntatore del mouse, usato per la direzione in cui lancio gli oggetti
    private Vector2 getAimDirection()
    {
        Vector3 shootDirection;
        shootDirection = Input.mousePosition;
        shootDirection.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        shootDirection = shootDirection - firePoint.transform.position;
        return new Vector2(shootDirection.x, shootDirection.y).normalized;
    }
    //Ad un command non si può passare un gameobject / prefab da spawnare, quindi ho fatto un metodo specifico per la bomba
    //un'alternativa brutta è fare un Resource.load() del prefab
    [Command]
    void Cmd_ThrowBomb()
    {
        GameObject obj = (GameObject)Instantiate(BombRBPrefab, firePoint.position, firePoint.rotation);
        NetworkServer.Spawn(obj);
        obj.GetComponent<Bomb>().addVelocity(shootDirection);
    }
}
