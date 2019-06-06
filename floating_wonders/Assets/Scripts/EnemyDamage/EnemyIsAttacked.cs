using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyIsAttacked : Strikeable
{
    private Rigidbody2D rb;
    [SerializeField] public float knockbackForce = 20f;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Strike(GameObject attacker)
    {
        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null)
        {
            if (attacker.transform.position.x > transform.position.x)
                rb.velocity = new Vector2(-1, 1) * 10;
            else
                rb.velocity = new Vector2(1, 1) * 10;
            health.TakeDamage(1);
        }
    }

}
