using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnlyShoot : MonoBehaviour
{
    private RaycastHit2D enemyToPlayerHit;
    private LayerMask mask;

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float sightSee;
    private float timeBtwShots;


    private float timer;
    [SerializeField]
    private float shootWaitingTime = 2;

    private Vector3 enemyToPlayerVector;


    void Start()
    {
        // Take the number corresponding to the "Player" layer
        mask = LayerMask.NameToLayer("Player");
    }

    void Update()
    {
        // If the distance between enemy and player is less than sightSee 
        if (Vector2.Distance(transform.position, player.transform.position) <= sightSee)
        {
            // Raycast between enemy and player
            enemyToPlayerHit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, sightSee);
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            Debug.Log(enemyToPlayerHit.collider.name);
            // Build new Vector3 from Enemy to Player. This is used to pass the initial player position in the bullet script
            enemyToPlayerVector = new Vector3((player.transform.position - transform.position).x, (player.transform.position - transform.position).y, 0);
            // If the raycast from enemy to player collide with a player, the enemy will shoot
            if (enemyToPlayerHit.transform.gameObject.layer == mask)
            {
                Debug.Log(enemyToPlayerHit.collider.name);
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
                Fire();
            }
        }
    }

    public void Fire()
    {
        // Timer to fire every shootWaitingTime seconds
        timer += Time.deltaTime;
        if (timer > shootWaitingTime)
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletInstance.GetComponent<Bullet>().RecieveBulletParameter(enemyToPlayerVector);
            timer = 0;
        }  
    }

    
}
