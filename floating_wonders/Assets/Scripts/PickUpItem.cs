using UnityEngine;
using UnityEngine.Networking;

public class PickUpItem : NetworkBehaviour
{

    private bool pickUpAllowed; //true quando il giocatore è in contatto con un oggetto che si può raccogliere
    private GameObject collidedObject; //con quale oggetto il giocatore è entrato in contatto

    private void Update()
    {
        if (pickUpAllowed && isLocalPlayer && Input.GetKey(KeyCode.E))
            Cmd_Pickup(collidedObject);
    }

    //quando il giocatore collide con un oggetto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Tool") && isLocalPlayer)
        {
            pickUpAllowed = true;
            collidedObject = collision.gameObject;
        }
    }

    //quando il giocatore esce dall'area di collisione
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Tool") && isLocalPlayer)
        {
            pickUpAllowed = false;
            collidedObject = collision.gameObject;
        }
    }

    [Command]
    void Cmd_Pickup(GameObject obj)
    {
        Destroy(obj);
    }
}
