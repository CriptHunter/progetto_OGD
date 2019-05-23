using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    private PlayersSharedHealth playerHealth;
    //se un giocatore entra in collisione con un altro
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<AnotherCharacterController>() != null)
        {
            GameObject networkManager = GameObject.Find("NetworkManager");
            playerHealth = networkManager.GetComponent<PlayersSharedHealth>();
            playerHealth.TakeDamage(damage);
        }
    }
}
