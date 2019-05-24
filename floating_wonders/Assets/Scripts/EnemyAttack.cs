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
        if (collision.gameObject.GetComponent<AnotherCharacterController>() != null)
        {
            //controllo solo sul server la collisione con il nemico perchè il player del client è presente anche sul server
            if (isServer)
            {
                GameManager.Instance.TakeDamage(damage);
                //calcolo in che direzione sono stato colpito dal nemico per spingere via il player dopo l'impatto
                //controllo solo destra o sinistra, se l'impatto arriva dall'alto considero sempre se è più a destra o più a sinistra
                var direction = transform.InverseTransformPoint(collision.transform.position);
                //destra
                if (direction.x > 0f)
                    Rpc_ApplyImpulse(collision.gameObject, 45, 25);
                //sinistra
                else if (direction.x < 0f)
                    Rpc_ApplyImpulse(collision.gameObject, 135, 25);
            }
        }
    }
    
    [ClientRpc] private void Rpc_ApplyImpulse(GameObject player, float direction, float strength)
    {
        player.GetComponent<AnotherCharacterController>().ApplyImpulse(direction, strength);
    }
}

