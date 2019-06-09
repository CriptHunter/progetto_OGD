using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Crate : Strikeable
{
    private bool collidingWithPlayer;
    private Rigidbody2D rb;
    private GameObject player;
    private Vector2 spawnPosition;

    private void Start()
    {
        spawnPosition = this.transform.position;
    }


    public override void Strike(GameObject attacker, int damage)
    {
        if (attacker.transform.position.x < transform.position.x)
            GetComponent<Rigidbody2D>().velocity = Vector2.right * 10;
        else
            GetComponent<Rigidbody2D>().velocity = Vector2.left * 10;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "DeathByFall")
            this.transform.position = spawnPosition;
    }
}
