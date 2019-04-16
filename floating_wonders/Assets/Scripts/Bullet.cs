using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector2 target;
    private LayerMask playerMask;
    private LayerMask enemyMask;
    private bool set = false;
    private Vector3 enemyPlayer;
    [SerializeField]
    private float destroyDelay = 5;
    private float timer;
    

    void Start()
    {
        // Take the number corresponding to the "Player" and the "Enemy" layers
        playerMask = LayerMask.NameToLayer("Player");
        enemyMask = LayerMask.NameToLayer("Enemy");

        target = new Vector2(player.position.x, player.position.y);
    }

    void Update()
    {
        // Calculate a position between the points specified by current and target, moving no farther than the distance specified by maxDistanceDelta.
        transform.position = Vector2.MoveTowards(transform.position, enemyPlayer + transform.position, speed * Time.deltaTime);
        TimerDestroy();
    }

    // Sent when another object enters a trigger collider attached to this object
    void OnTriggerEnter2D(Collider2D other)
    {   
        // If the bullet doesn't collide with an Enemy, the bullet will be destroyed. Otherwise the bullet will be destroyed at bullet spawn.
        if(other.transform.gameObject.layer != enemyMask)
            DestroyBullet();
        // If the bullet collide with a Player it will damage the player healt.
        if (other.transform.gameObject.layer == playerMask)
        {
            //Damage the player
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    // Called by other scripts to set the initial player position
    public void RecieveBulletParameter(Vector3 enemyPlayer)
    {
        this.enemyPlayer = enemyPlayer;
    }

    // Timer to destroy bullet after destroyDelay seconds
    public void TimerDestroy()
    {
        timer += Time.deltaTime;
        if (timer > destroyDelay)
        {
            DestroyBullet();
            timer = 0;
        }
    }
}
