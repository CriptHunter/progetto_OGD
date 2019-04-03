using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    [SerializeField] [SyncVar(hook = "OnChangeHealth")] private int currentHealth;

    public void TakeDamage(int damage)
    {
        if (!isServer)
            return;
        currentHealth = currentHealth - damage;
    }

    //viene chiamato ogni volta che la vita cambia

    void OnChangeHealth(int health)
    {
        print("vita corrente: " + health);
    }
}
