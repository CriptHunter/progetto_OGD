using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnlyShoot : MonoBehaviour
{
    private RaycastHit2D hit;
    private LayerMask playerMask;
    private LayerMask enemyMask;

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject player;
    [SerializeField] private float sightSee = 10;
    [SerializeField] private float shootWaitingTime = 2;
    private float timeBtwShots;
    private float timer;
    private Vector3 enemyToPlayerVector;


    void Start()
    {
        // Take the number corresponding to the "Player" layer
        playerMask = LayerMask.NameToLayer("Player");
        enemyMask = ~(1 << 10);
    }

    void Update()
    {
        // If the distance between enemy and player is less than sightSee 
        if (Vector2.Distance(transform.position, player.transform.position) <= sightSee)
        {
            timer += Time.deltaTime;
            //raycast a cerchio, ritorna il primo oggetto colpito
            hit = Physics2D.CircleCast(transform.position, sightSee, Vector2.zero, enemyMask);
            // If the raycast from enemy to player collide with a player, the enemy will shoot
            if (hit.collider != null && hit.transform.gameObject.GetComponent<AnotherCharacterInput>() != null)
            {
                // Build new Vector3 from Enemy to Player. This is used to pass the initial player position in the bullet script
                enemyToPlayerVector = hit.point - new Vector2(transform.position.x, transform.position.y);
                Fire();
            }
        }
    }

    public void Fire()
    {
        // Timer to fire every shootWaitingTime seconds
        if (timer > shootWaitingTime)
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletInstance.GetComponent<Bullet>().RecieveBulletParameter(enemyToPlayerVector);
            timer = 0;
        }  
    }
}
