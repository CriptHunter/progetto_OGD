﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    [SerializeField] [SyncVar(hook = "OnChangeHealth")] private int health;

    public void TakeDamage(int damage)
    {
        if (!isServer)
            return;
        health = health - damage;
    }

    //viene chiamato ogni volta che la vita cambia
    void OnChangeHealth(int newHealth)
    {
        print("vita corrente: " + newHealth);
    }
}
