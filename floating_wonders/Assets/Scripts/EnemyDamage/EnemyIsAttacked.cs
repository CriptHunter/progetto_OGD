using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            health.TakeDamage(1);
            /*if (attacker.transform.position.x > transform.position.x)
                rb.MovePosition((Vector2)transform.position + Vector2.left * 5);
            else
                rb.MovePosition((Vector2)transform.position + Vector2.right * 5);*/
        }
    }
}
