using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ItemManager : NetworkBehaviour
{
    private GameObject pickedUpItem; //quale oggetto è stato raccolto
    private ItemType uniqueItem; //oggetto caratteristico del personaggio
    private Vector2 shootDirection; //vettore che indica in quale direzione vado a lanciare l'oggetto
    private bool canShoot;
    private bool pickupAllowed;
    private GameObject collidedObject;
    private AnotherCharacterController controller;
    private new BoxCollider2D collider;
    private DrawCircle drawCircle;
    [SerializeField] private Transform firePoint = null; // da quale punto sono lanciati gli oggetti
    [SerializeField] private GameObject bombRBPrefab = null; //bomba con rigid body


    private void Start()
    {
        pickedUpItem = null;
        controller = GetComponent<AnotherCharacterController>();
        collider = GetComponents<BoxCollider2D>()[0];
        drawCircle = GetComponentInChildren<DrawCircle>();
    }

    private void Update()
    {
        if (controller.IsBeingHeldByCharacter() || controller.IsHoldingCharacter())
            firePoint.GetComponent<SpriteRenderer>().enabled = false;
        //se posso raccogliere qualcosa e premo E --> raccoglie oggetto
        if (Input.GetKeyDown(KeyCode.E) && isLocalPlayer && pickupAllowed)
        {
            Pickup(collidedObject);
        }
        //tenendo premuto R si mira
        else if (Input.GetKey(KeyCode.Mouse1) && isLocalPlayer && !(controller.IsHoldingCharacter() || controller.IsBeingHeldByCharacter() || controller.IsClimbing() || controller.IsDanglingFromEdge()))
        {
            MarkInteractiveObject();
            shootDirection = GetAimDirection();
            MoveArrowPointer(GetShootingAngle(shootDirection));
            drawCircle.ShowCircle();
        }
        //quando rilascio R --> usa l'oggetto se l'angolazione è valida
        else if (Input.GetKeyUp(KeyCode.Mouse1) && isLocalPlayer && !(controller.IsHoldingCharacter() || controller.IsBeingHeldByCharacter() || controller.IsClimbing() || controller.IsDanglingFromEdge()))
        {
            MarkInteractiveObject();
            //se lo sprite della freccia per mirare è visibile allora l'angolo è tra -90 e 90
            if (canShoot)
                Shoot();
            firePoint.GetComponent<SpriteRenderer>().enabled = false;
            drawCircle.HideCircle();
        }
    }

    public void SetUniqueItem(ItemType uniqueItem)
    {
        this.uniqueItem = uniqueItem;
    }

    public void Pickup(GameObject collidedObject)
    {
        //se sto raccogliendo un collezionabile
        if (collidedObject.GetComponent<Pickuppable>().collectible)
            Cmd_PickupItem(collidedObject);
        //se sto raccogliendo un giocatore o un oggetto
        else if (pickedUpItem == null)
        {
            //salvo quale oggetto ho raccolto, serve per quando viene usato
            pickedUpItem = collidedObject;
            Cmd_PickupItem(pickedUpItem);
        }
    }

    //quando il giocatore collide con un oggetto, controllo solo il collider capsula
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se collido con un oggetto che si può raccogliere, sono il local player e non ho oggetti in mano ---> posso raccogliere
        if (collision.gameObject.GetComponent<Pickuppable>() != null && isLocalPlayer && collider.IsTouching(collision))
        {
            pickupAllowed = true;
            collidedObject = collision.gameObject;
        }
    }

    //quando il giocatore esce dall'area di collisione, controllo solo il collider capsula
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Pickuppable>() != null && isLocalPlayer && !collider.IsTouching(collision))
        {
            pickupAllowed = false;
            collidedObject = null;
        }
    }

    //usa l'oggetto equipaggiato
    private void Shoot()
    {
        //se nessun oggetto è equipaggiato usa i tool base
        if (pickedUpItem == null)
        {
            if (uniqueItem == ItemType.extendableArm)
                this.gameObject.GetComponent<ExtendableArm>().Throw(shootDirection);
            else
                this.gameObject.GetComponent<GrapplingHook>().Throw(shootDirection);
        }
        else
        {
            switch (pickedUpItem.GetComponent<Pickuppable>().Type)
            {
                case ItemType.bomb:
                    Cmd_ThrowBomb(shootDirection);
                    break;
            }
            pickedUpItem = null;
        }
    }

    //restituisce un vettore 2D che va dal fire point al puntatore del mouse, usato per la direzione in cui lancio gli oggetti
    private Vector2 GetAimDirection()
    {
        /*Vector3 shootDirection;
        shootDirection = Input.mousePosition;
        shootDirection.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        shootDirection = shootDirection - firePoint.transform.position;*/
        shootDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(firePoint.position);
        return (Vector2)shootDirection.normalized;
    }

    //ruota il puntatore a forma di freccia in modo da seguire il cursore del mouse
    private float GetShootingAngle(Vector2 shootDirection)
    {
        float shootingAngle = 0f;
        //calcolo l'angolo tra l'asse x e il vettore della direzione di tiro
        if (GetComponent<AnotherCharacterController>().GetVerse() == Verse.Right)
            shootingAngle = Vector2.SignedAngle(shootDirection, transform.right);
        else if (GetComponent<AnotherCharacterController>().GetVerse() == Verse.Left)
            shootingAngle = Vector2.SignedAngle(shootDirection, -transform.right);
        return -shootingAngle;

    }

    private void MoveArrowPointer(float shootingAngle)
    {
        //ruoto la freccia sull'asse Z di un valore pari all'angolo
        firePoint.eulerAngles = new Vector3(0, 0, shootingAngle);
        //rendo la freccia visibile solo se posso sparare (dipende dall'angolo)
        firePoint.GetComponent<SpriteRenderer>().enabled = CanShoot(shootingAngle);
    }

    private bool CanShoot(float shootingAngle)
    {
        if (shootingAngle > 90 || shootingAngle < -90)
            canShoot = false;
        else
            canShoot = true;
        return canShoot;
    }

    private void MarkInteractiveObject()
    {
        if (!canShoot)
            return;
        LayerMask ignoredLayer;
        bool interestingHit = false;
        if (uniqueItem == ItemType.grapplingHook)
        {
            ignoredLayer = ~((1 << 2) | (1 << 9) | (1 << 11) | (1 << 12) | (1 << 13));
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, shootDirection, GetComponent<GrapplingHook>().GetMaxDistance(), ignoredLayer);
            if (hit.transform != null && hit.transform.tag == "HookPoint")
                interestingHit = true;
            else
                interestingHit = false;
        }
        else
        {
            ignoredLayer = ~((1 << 2) | (1 << 9) | (1 << 11) | (1 << 12));
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, shootDirection, GetComponent<ExtendableArm>().GetMaxDistance(), ignoredLayer);
            if(hit.transform != null && (hit.transform.gameObject.layer == LayerMask.NameToLayer("Items") || hit.transform.tag == "Crate"))
                interestingHit = true;
            else
                interestingHit = false;
        }

        if(interestingHit)
        {
            firePoint.GetComponent<SpriteRenderer>().color = Color.red;
            drawCircle.SetLineColor(Color.red);
        }
        else
        {
            firePoint.GetComponent<SpriteRenderer>().color = Color.white;
            drawCircle.SetLineColor(Color.white);
        }
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
}