﻿using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ItemManager : NetworkBehaviour
{
    private bool pickUpAllowed; //true quando il giocatore è in contatto con un oggetto che si può raccogliere
    private GameObject collidedObject; //con quale gameobject il giocatore è entrato in contatto
    public GameObject pickedUpItem; //quale oggetto è stato raccolto
    public ItemType uniqueItem; //oggetto caratteristico del personaggio
    private Vector2 shootDirection; //vettore che indica in quale direzione vado a lanciare l'oggetto
    private new CircleCollider2D collider; //collider usato da raycast per vedere se di fronte c'è un altro player
    private LayerMask raycastPlayerOnlyMask = (1 << 9);
    [SerializeField] private Transform firePoint = null; // da quale punto sono lanciati gli oggetti
    [SerializeField] private GameObject bombRBPrefab = null; //bomba con rigid body


    private void Start()
    {
        pickedUpItem = null;
        uniqueItem = ItemType.nullItem;
        collider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        //se posso raccogliere qualcosa e premo E --> raccoglie oggetto
        if (Input.GetKeyDown(KeyCode.E) && isLocalPlayer)
        {
            if(pickUpAllowed)
                Pickup(collidedObject);
            else if (checkForPlayer())
                Pickup(collidedObject);
        }

  

        //se ho un oggetto in mano e tengo premuto R --> entra in fase di mira
        else if (Input.GetKey(KeyCode.R) && isLocalPlayer)
        {
            shootDirection = GetAimDirection();
            RotateArrowPointer(shootDirection);
        }

        //quando rilascio R --> usa l'oggetto se l'angolazione è valida
        else if (Input.GetKeyUp(KeyCode.R) && isLocalPlayer)
        {
            //se lo sprite della freccia per mirare è visibile allora l'angolo è tra -90 e 90
            if (firePoint.GetComponent<SpriteRenderer>().enabled)
                useItem();
            firePoint.GetComponent<SpriteRenderer>().enabled = false;
        }
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
            //se sto raccogliendo un giocatore disattivo l'input di movimento di entrambi, e l'item manager del giocatore raccolto
            if (collidedObject.GetComponent<AnotherCharacterController>() != null)
            {
                this.gameObject.GetComponent<AnotherCharacterInput>().enabled = false;
                Cmd_CharacterInputEnabled(pickedUpItem, false);
                Cmd_ItemManagerEnabled(pickedUpItem, false);
                //Cmd_SetRigidBody(pickedUpItem, false);
                //Cmd_SetPosition(pickedUpItem, firePoint.position);
                pickedUpItem.transform.position = firePoint.position;
                Cmd_MoveTo(pickedUpItem, firePoint.position);
            }
            //se sto raccogliendo un oggetto
            else
                Cmd_PickupItem(pickedUpItem);
        }
    }

    //quando il giocatore collide con un oggetto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se collido con un oggetto che si può raccogliere, sono il local player e non ho oggetti in mano ---> posso raccogliere
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
            collidedObject = null;
        }
    }

    //raycast per vedere se di fronte c'è un giocatore, 
    private bool checkForPlayer()
    {
        if (isLocalPlayer)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(collider.bounds.center, collider.radius, Vector2.right, 0.0f, raycastPlayerOnlyMask);
            foreach (RaycastHit2D h in hits)
            {
                if (h.collider.gameObject != this.gameObject)
                {
                    collidedObject = h.collider.gameObject;
                    print("player colpito da circle raycast");
                    return true;
                }
            }
            return false;
        }
        return false;
    }


    //usa l'oggetto equipaggiato
    private void useItem()
    {
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
                case ItemType.player:
                    //Cmd_SetRigidBody(pickedUpItem, true);
                    Cmd_ApplyImpulse(pickedUpItem, shootDirection.GetAngle(), 30);
                    Cmd_CharacterInputEnabled(pickedUpItem, true);
                    Cmd_ItemManagerEnabled(pickedUpItem, true);
                    this.gameObject.GetComponent<AnotherCharacterInput>().enabled = true;                  
                    break;
            }
            pickedUpItem = null;
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
        float shootingAngle = 0f;
        //calcolo l'angolo tra l'asse x e il vettore della direzione di tiro
        //se il personaggio è girato verso destra
        if (GetComponent<AnotherCharacterController>().GetVerse() == Verse.Right)
            shootingAngle = Vector2.SignedAngle(shootDirection, transform.right);
        //se il personaggio è girato verso sinistra
        else if (GetComponent<AnotherCharacterController>().GetVerse() == Verse.Left)
            shootingAngle = Vector2.SignedAngle(shootDirection, -transform.right);
        shootingAngle = -shootingAngle;
        //ruoto la freccia sull'asse Z di un valore pari all'angolo
        firePoint.eulerAngles = new Vector3(0, 0, shootingAngle);
        if (shootingAngle > 90 || shootingAngle < -90)
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

    [Command] private void Cmd_ApplyImpulse(GameObject player, float direction, float strength)
    {
        Rpc_ApplyImpulse(player, direction, strength);
    }

    [ClientRpc] private void Rpc_ApplyImpulse(GameObject player, float direction, float strength)
    {
        player.gameObject.GetComponent<AnotherCharacterController>().ApplyImpulse(direction, strength);
    }

    [Command] private void Cmd_CharacterInputEnabled(GameObject player, bool enabled)
    {
        Rpc_CharacterInputEnabled(player, enabled);   
    }

    [ClientRpc] private void Rpc_CharacterInputEnabled(GameObject player, bool enabled)
    {
        player.gameObject.GetComponent<AnotherCharacterInput>().enabled = enabled;
    }

    [Command] private void Cmd_ItemManagerEnabled(GameObject player, bool enabled)
    {
        Rpc_ItemManagerEnabled(player, enabled);
    }

    [ClientRpc] private void Rpc_ItemManagerEnabled(GameObject player, bool enabled)
    {
        player.gameObject.GetComponent<ItemManager>().enabled = enabled;
    }

    [Command] private void Cmd_SetRigidBody(GameObject player, bool active)
    {
        Rpc_SetRigidBody(player, active);
    }

    [ClientRpc] private void Rpc_SetRigidBody(GameObject player, bool active)
    {
        player.gameObject.GetComponent<AnotherCharacterController>().Activate(active);
        if (!active)
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        else
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    [Command]
    public void Cmd_MoveTo(GameObject player, Vector2 newPosition)
    {
        player.transform.position = newPosition;
        Rpc_MoveTo(player, newPosition);
    }
    [ClientRpc]
    void Rpc_MoveTo(GameObject player, Vector2 newPosition)
    {
        player.transform.position = newPosition;
    }
}