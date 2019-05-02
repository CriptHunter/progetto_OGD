using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PickUpThrow : NetworkBehaviour
{
    private bool pickUpAllowed; //true quando il giocatore è in contatto con un oggetto che si può raccogliere
    private GameObject collidedObject; //con quale gameobject il giocatore è entrato in contatto
    private GameObject pickedUpItem = null; //quale oggetto è stato raccolto
    private Vector2 shootDirection; //vettore che indica in quale direzione vado a lanciare l'oggetto
    [SerializeField] private Transform firePoint = null; // da quale punto usa l'oggetto
    [SerializeField] private GameObject bombRBPrefab = null; //bomba con rigid body

    private void Update()
    {
        //se posso raccogliere qualcosa e premo E --> raccoglie oggetto
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E) && isLocalPlayer)
        {
            pickedUpItem = collidedObject;
            Cmd_PickupItem(pickedUpItem);
        }

        //se ho un oggetto in mano e tengo premuto R --> entra in fase di mira
        else if (pickedUpItem != null && Input.GetKey(KeyCode.R) && isLocalPlayer)
        {
            shootDirection = GetAimDirection();
            RotateArrowPointer(shootDirection);
        }

        //quando rilascio R --> usa l'oggetto se l'angolazione è valida
        else if (pickedUpItem != null && Input.GetKeyUp(KeyCode.R) && isLocalPlayer)
        {
            //se lo sprite della freccia per mirare è visibile allora l'angolo è tra -90 e 90
            if (firePoint.GetComponent<SpriteRenderer>().enabled)
                useItem();
            firePoint.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    //quando il giocatore collide con un oggetto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se collido con un oggetto che si può raccogliere, sono il local player e non ho oggetti in mano ---> posso raccogliere
        if (collision.gameObject.GetComponent<Pickuppable>() != null && isLocalPlayer && pickedUpItem == null)
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
            collidedObject = null;
        }
    }

    //se un giocatore entra in collisione con un altro
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Pickuppable>() != null && isLocalPlayer)
        {
            pickUpAllowed = true;
            collidedObject = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Pickuppable>() != null && isLocalPlayer)
        {
            pickUpAllowed = false;
            collidedObject = null;
        }
    }*/

    //usa l'oggetto equipaggiato
    private void useItem()
    {
        switch (pickedUpItem.GetComponent<Pickuppable>().Type)
        {
            case EnumCollection.ItemType.bomb:
                Cmd_ThrowBomb(shootDirection);
                Cmd_Respawn(pickedUpItem, 3);
                pickedUpItem = null;
                break;
            case EnumCollection.ItemType.grapplingHook:
                this.gameObject.GetComponent<GrapplingHook>().Throw(shootDirection);
                break;
            case EnumCollection.ItemType.player:
                break;
            case EnumCollection.ItemType.extendableArm:
                this.gameObject.GetComponent<ExtendableArm>().Throw(shootDirection);
                break;
        }
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
        //se il personaggio è girato verso destra
        if (GetComponent<AnotherCharacterController>().GetVerse() == Verse.Right)
            angle = Vector2.SignedAngle(shootDirection, transform.right);
        //se il personaggio è girato verso sinistra
        else if (GetComponent<AnotherCharacterController>().GetVerse() == Verse.Left)
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
        //quaternion.identity è rotazione pari a 0,0,0
        GameObject obj = (GameObject)Instantiate(bombRBPrefab, firePoint.position, Quaternion.identity);
        NetworkServer.Spawn(obj);
        obj.GetComponent<Bomb>().AddVelocity(shootDirection);
    }

    [Command]
    private void Cmd_PickupItem(GameObject item)
    {
        item.GetComponent<Pickuppable>().Pickup();
    }

    [Command]
    private void Cmd_Respawn(GameObject item, int respawnTime)
    {
        item.GetComponent<Pickuppable>().Cmd_Respawn(respawnTime);
    }
}