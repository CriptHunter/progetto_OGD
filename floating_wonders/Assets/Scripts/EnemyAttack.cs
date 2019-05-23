using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyAttack : NetworkBehaviour
{
    [SerializeField] private int damage = 1;
    int i = 0;
    //se un giocatore entra in collisione con un altro
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<AnotherCharacterController>() != null)
        {
            //controllo solo sul server la collisione con il nemico perchè il player del client è presente anche sul server
            if (isServer)
                GameManager.Instance.TakeDamage(2);
        }
    }
}
