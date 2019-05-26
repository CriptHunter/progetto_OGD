using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIsAttacked : Strikeable
{
    [SerializeField] public float knockbackForce = 0f;
    public override void Strike(GameObject attacker)
    {
        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.TakeDamage(1);
            print(attacker);
           /* var direction = transform.InverseTransformPoint(attacker.transform.position);
            //destra
            if (direction.x > 0f)
                print("destra");
            //sinistra
            else if (direction.x < 0f)
                print("sinistra");*/
        }
    }
}
