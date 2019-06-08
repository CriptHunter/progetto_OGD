using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyAttack : NetworkBehaviour
{
    [SerializeField] private int damage = 1;
    private float timer = 0;
    //se un giocatore entra in collisione con un altro
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isServer)
            return;

        if (collision.gameObject.GetComponent<SetupLocalPlayer>() != null)
        {
            GameManager.Instance.TakeDamage(collision.gameObject, damage);
            //calcolo in che direzione ha attaccato il nemico per spingere via il player dopo l'impatto
            //controllo solo destra o sinistra, se l'impatto arriva dall'alto considero sempre se è più a destra o più a sinistra
            //destra
            if (collision.transform.position.x > transform.position.x)
                Rpc_ApplyImpulse(collision.gameObject, 45, 15);
            //sinistra
            else
                Rpc_ApplyImpulse(collision.gameObject, 135, 15);
        }
    }

    /*private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isServer)
            return;
        print("timer " + timer);
        if (collision.gameObject.GetComponent<SetupLocalPlayer>() != null)
        {
            timer = timer + Time.deltaTime;
            if ((timer == 0 || timer > 0.5f))
            {
                GameManager.Instance.TakeDamage(collision.gameObject, damage);
                //calcolo in che direzione ha attaccato il nemico per spingere via il player dopo l'impatto
                //controllo solo destra o sinistra, se l'impatto arriva dall'alto considero sempre se è più a destra o più a sinistra
                //destra
                if (collision.transform.position.x > transform.position.x)
                    Rpc_ApplyImpulse(collision.gameObject, 45, 15);
                //sinistra
                else
                    Rpc_ApplyImpulse(collision.gameObject, 135, 15);
                timer = 0;
            }
        }       
    }*/

    [ClientRpc] private void Rpc_ApplyImpulse(GameObject player, float direction, float strength)
    {
        player.GetComponent<AnotherCharacterController>().ApplyImpulse(direction, strength);
    }
}

