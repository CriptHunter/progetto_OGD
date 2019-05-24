using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIsAttacked : Strikeable
{
    [SerializeField] public float knockbackForce = 0f;
    public override void Strike()
    {
        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.TakeDamage(1);
        }
    }
}
