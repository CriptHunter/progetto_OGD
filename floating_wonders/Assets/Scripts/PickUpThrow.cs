using UnityEngine;
using UnityEngine.Networking;

public class PickUpThrow : NetworkBehaviour
{
    private bool pickUpAllowed; //true quando il giocatore è in contatto con un oggetto che si può raccogliere
    private GameObject collidedObject; //con quale gameobject il giocatore è entrato in contatto
    private int pickedUpItem = -1; //indica il tipo di oggetto raccolto
    public int PickedUpItem { get { return pickedUpItem; } }
    private void Update()
    {
        if (pickUpAllowed && isLocalPlayer && Input.GetKey(KeyCode.E))
        {
            Cmd_Pickup(collidedObject);
            pickedUpItem = (int)collidedObject.GetComponent<Pickuppable>().Type;
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

    //chiede al server di distruggere l'oggetto
    [Command]
    void Cmd_Pickup(GameObject obj)
    {
        Destroy(obj);
    }
}
