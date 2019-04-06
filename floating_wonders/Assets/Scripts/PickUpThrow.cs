using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class PickUpThrow : NetworkBehaviour
{
    private bool pickUpAllowed; //true quando il giocatore è in contatto con un oggetto che si può raccogliere
    private GameObject collidedObject; //con quale gameobject il giocatore è entrato in contatto
    private int pickedUpItem = -1;
    [SerializeField] private Transform firePoint; // da quale punto usa l'oggetto
    private void Update()
    {
        //se posso raccogliere qualcosa e premo E --> raccoglie oggetto
        if (pickUpAllowed && Input.GetKey(KeyCode.E) && isLocalPlayer)
        {
            pickedUpItem = (int)collidedObject.GetComponent<Pickuppable>().Type;
            Cmd_Pickup(collidedObject);
        }

        //se non posso raccogliere ma ho un oggetto in mano ---> usa oggetto
        else if (pickedUpItem >= 0 && Input.GetKey(KeyCode.R) && isLocalPlayer)
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

    private void useItem()
    {
        switch (pickedUpItem)
        {
            case 0:
                print("uso bomba");
                break;
            case 1:
                print("uso oggetto 1");
                break;
            case 2:
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

}
