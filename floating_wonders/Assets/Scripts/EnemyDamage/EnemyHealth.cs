using Spine.Unity;
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
        print("vita corrente: " + health);
        if (health <= 0)
            this.gameObject.SetActive(false);
        else if(health != maxHealth)
            StartCoroutine(Stun(this.gameObject));
    }

    public IEnumerator Stun(GameObject enemy)
    {
        print("inizio coroutine");
        Rpc_SetSpineColor(enemy, Color.red);
        yield return new WaitForSeconds(.3f);
        Rpc_SetSpineColor(enemy, Color.white);
    }

    [ClientRpc]
    public void Rpc_SetSpineColor(GameObject enemy, Color color)
    {
        print("client rpc");
        enemy.GetComponentInChildren<SkeletonAnimation>().skeleton.SetColor(color);
    }
}
