using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyHit : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hit = collision.gameObject;
        Health health = hit.GetComponent<Health>();
        if (health != null)
            health.TakeDamage(1);
        //spinge via il player
        Rigidbody2D playerRigidBody = hit.GetComponent<Rigidbody2D>();
        //controlla se il nemico ha colpito da destra o da sinistra
        float xDir = collision.transform.position.x - transform.position.x;
        if (xDir > 0)
            playerRigidBody.velocity = new Vector2(80, 0);
        else
            playerRigidBody.velocity = new Vector2(-80, 0);
        
    }
}
