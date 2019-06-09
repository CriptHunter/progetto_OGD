﻿using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyHealth : NetworkBehaviour
{
    [SerializeField] private int maxHealth;
    [SyncVar(hook = "OnChangeHealth")] private int health;

    private void Start()
    {
        this.health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!isServer)
            return;
        health = health - damage;
    }

    public void SetFullHealth()
    {
        health = maxHealth;
    }

    //viene chiamato ogni volta che la vita cambia
    private void OnChangeHealth(int newHealth)
    {
        health = newHealth;
        print("vita nemico: " + this.gameObject + " = " + health);
        if (health <= 0)
            this.gameObject.SetActive(false);
        else if(health != maxHealth)
            StartCoroutine(Stun());
    }

    public IEnumerator Stun()
    {
        GetComponentInChildren<SkeletonAnimation>().skeleton.SetColor(Color.red);
        yield return new WaitForSeconds(.3f);
        GetComponentInChildren<SkeletonAnimation>().skeleton.SetColor(Color.white);
    }
}
