using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Crate : Strikeable
{
    private bool collidingWithPlayer;
    private Rigidbody2D rb;
    private GameObject player;

    public override void Strike(GameObject attacker)
    {
        if (attacker.transform.position.x < transform.position.x)
            GetComponent<Rigidbody2D>().velocity = Vector2.right * 10;
        else
            GetComponent<Rigidbody2D>().velocity = Vector2.left * 10;
    }
}
