using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PickUpThrow : NetworkBehaviour
{
    private bool pickUpAllowed; //true quando il giocatore è in contatto con un oggetto che si può raccogliere
    private GameObject collidedObject; //con quale gameobject il giocatore è entrato in contatto
    private int pickedUpItemType = (int)EnumCollection.itemsEnum.nullItem; //che tipo è stato raccolto, -1 --> nessun oggetto
    private GameObject pickedUpItem = null; //quale oggetto è stato raccolto
    private Vector2 shootDirection; //vettore che indica in quale direzione vado a lanciare l'oggetto
    [SerializeField] private Transform firePoint = null; // da quale punto usa l'oggetto
    [SerializeField] private GameObject bombRBPrefab = null; //bomba con rigid body

    private void Update()
    {
        //se posso raccogliere qualcosa e premo E --> raccoglie oggetto
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E) && isLocalPlayer)
        {
            pickedUpItemType = (int)collidedObject.GetComponent<Pickuppable>().Type;
            pickedUpItem = collidedObject;
            pickedUpItem.GetComponent<Pickuppable>().Pickup();
        }

        //se o un oggetto in mano e tengo premuto R --> entra in fase di mira
        else if (pickedUpItemType >= 0 && Input.GetKey(KeyCode.R) && isLocalPlayer)
        {
            shootDirection = GetAimDirection();
            RotateArrowPointer(shootDirection);
        }

        //quando rilascio R --> usa l'oggetto
        else if (pickedUpItemType >= 0 && Input.GetKeyUp(KeyCode.R) && isLocalPlayer)
        {
            useItem();
            firePoint.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    //quando il giocatore collide con un oggetto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se collido con un oggetto che si può raccogliere, sono il local player e non ho oggetti in mano ---> posso raccogliere
        if (collision.gameObject.GetComponent<Pickuppable>() != null && isLocalPlayer && pickedUpItemType == (int)EnumCollection.itemsEnum.nullItem)
        {
            pickUpAllowed = true;
            collidedObject = collision.gameObject;
            Cmd_SetAuthority(collidedObject.GetComponent<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());
        }
    }

    //quando il giocatore esce dall'area di collisione
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Pickuppable>() != null && isLocalPlayer)
        {
            pickUpAllowed = false;
            collidedObject = null;
        }
    }

    //usa l'oggetto equipaggiato
    private void useItem()
    {
        switch (pickedUpItemType)
        {
            case (int)EnumCollection.itemsEnum.bomb:
                Cmd_ThrowBomb(shootDirection);
                break;
            case (int)EnumCollection.itemsEnum.redItem:
                break;
            case (int)EnumCollection.itemsEnum.blueItem:
                break;        
        }
        pickedUpItem.GetComponent<Pickuppable>().Cmd_Respawn(5);
        Cmd_RemoveAuthority(pickedUpItem.GetComponent<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());
        pickedUpItemType = (int)EnumCollection.itemsEnum.nullItem;
    }

    //restituisce un vettore 2D che va dal fire point al puntatore del mouse, usato per la direzione in cui lancio gli oggetti
    private Vector2 GetAimDirection()
    {
        Vector3 shootDirection;
        shootDirection = Input.mousePosition;
        shootDirection.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        shootDirection = shootDirection - firePoint.transform.position;
        return new Vector2(shootDirection.x, shootDirection.y).normalized;
    }

    //ruota il puntatore a forma di freccia in modo da seguire il cursore del mouse
    private void RotateArrowPointer(Vector2 shootDirection)
    {
        float angle = 0f;
        //calcolo l'angolo tra l'asse x e il vettore della direzione di tiro
        //ci sono 2 casi: personaggio girato a destra o a sinistra
        if (GetComponent<AnotherCharacterController>().GetVerse() == Verse.Right)
            angle = Vector2.SignedAngle(shootDirection, transform.right);
        else if(GetComponent<AnotherCharacterController>().GetVerse() == Verse.Left)
            angle = Vector2.SignedAngle(shootDirection, -transform.right);
        angle = -angle;
        //ruoto la freccia sull'asse Z di un valore pari all'angolo
        firePoint.eulerAngles = new Vector3(0, 0, angle);
        if (angle > 90 || angle < -90)
            firePoint.GetComponent<SpriteRenderer>().enabled = false;
        else
            firePoint.GetComponent<SpriteRenderer>().enabled = true;
    }

    //Ad un command non si può passare un gameobject / prefab da spawnare, quindi ho fatto un metodo specifico per la bomba
    //un'alternativa brutta è fare un Resource.load() del prefab
    [Command]
    private void Cmd_ThrowBomb(Vector2 shootDirection)
    {
        GameObject obj = (GameObject)Instantiate(bombRBPrefab, firePoint.position, firePoint.rotation);
        NetworkServer.Spawn(obj);
        obj.GetComponent<Bomb>().AddVelocity(shootDirection);
    }

    //i client non possono chiamare command su oggetti di cui non hanno l'autority
    //questo metodo assegna l'autority (toglierla quando si ha finito di usare l'oggetto)
    //nel network manager va spuntata una roba sul local player
    [Command]
    void Cmd_SetAuthority(NetworkIdentity item, NetworkIdentity player)
    {
        bool b = item.AssignClientAuthority(player.connectionToClient);
    }

    [Command]
    void Cmd_RemoveAuthority(NetworkIdentity item, NetworkIdentity player)
    {
        bool b = item.RemoveClientAuthority(player.connectionToClient);
    }
}
