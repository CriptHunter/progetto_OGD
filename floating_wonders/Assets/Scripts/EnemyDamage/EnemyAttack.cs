using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyAttack : NetworkBehaviour
{
    [SerializeField] private int damage = 1;
    //se un giocatore entra in collisione con un altro
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<SetupLocalPlayer>() != null)
        {
            //controllo solo sul server la collisione con il nemico perchè il player del client è presente anche sul server
            if (isServer)
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
    }

    [ClientRpc] private void Rpc_ApplyImpulse(GameObject player, float direction, float strength)
    {
        player.GetComponent<AnotherCharacterController>().ApplyImpulse(direction, strength);
    }
}

