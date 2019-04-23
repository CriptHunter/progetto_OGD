/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickUpThrowPlayer : NetworkBehaviour
{
    private bool pickUpAllowed; //true quando il giocatore è in contatto con un altro personaggio
    private bool holding = false; //true se sto tenendo in braccio un altro giocatore
    private GameObject collidedObject; //con quale gameobject il giocatore è entrato in contatto
    private Vector2 shootDirection; //vettore che indica in quale direzione vado a lanciare l'oggetto
    [SerializeField] private Transform firePoint = null; // da quale punto tiene il giocatore

    private void Update()
    {
        //se sono vicino ad un personaggio e premo E --> raccoglie personaggio
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E) && isLocalPlayer)
        {
            
        }

        //se o un oggetto in mano e tengo premuto R --> entra in fase di mira
        else if (holding && Input.GetKey(KeyCode.R) && isLocalPlayer)
        {
            shootDirection = GetAimDirection();
            RotateArrowPointer(shootDirection);
        }

        //quando rilascio R --> usa l'oggetto se l'angolazione è valida
        else if (holding && Input.GetKeyUp(KeyCode.R) && isLocalPlayer)
        {
            //se lo sprite della freccia per mirare è visibile allora l'angolo è tra -90 e 90
            if (firePoint.GetComponent<SpriteRenderer>().enabled)
                print("lancio giocatore");
            firePoint.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    //quando il giocatore collide con un oggetto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //se collido con un player
        if (collision.gameObject.GetComponent<PlayerController>() != null && isLocalPlayer)
        {
            pickUpAllowed = true;
            collidedObject = collision.gameObject;
        }
    }

    //quando il giocatore esce dall'area di collisione
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null && isLocalPlayer)
        {
            pickUpAllowed = false;
            collidedObject = null;
        }
    }
}*/
