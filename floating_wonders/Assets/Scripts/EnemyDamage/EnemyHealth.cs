using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyHealth : NetworkBehaviour
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
        health = newHealth;
        print("vita corrente: " + health);
        if (health <= 0)
            this.gameObject.SetActive(false);
        else
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
