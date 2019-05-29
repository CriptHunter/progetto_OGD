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
            print(attacker);
            var direction = transform.InverseTransformPoint(attacker.transform.position);
            if (direction.x > 0f)
                rb.velocity = Vector2.left * knockbackForce;
            else if (direction.x < 0f)
                rb.velocity = Vector2.right * knockbackForce;
        }
    }
}
