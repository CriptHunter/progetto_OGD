using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyOnlyShoot : NetworkBehaviour
{
    private Collider2D[] hit;
    private LayerMask playerMask;
    private LayerMask enemyMask;
    private RaycastHit2D enemyPlayerHit;

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject player;
    [SerializeField] private float sightSee = 10;
    [SerializeField] private float shootWaitingTime = 2;
    private float timeBtwShots;
    private float timer;
    private Vector3 enemyToPlayerVector;
    private int i;
    private bool twoPlayers;

    void Start()
    {
        // Take the number corresponding to the "Player" layer
        playerMask = 1 << LayerMask.NameToLayer("Player");
        enemyMask = ~(1 << 10);
        twoPlayers = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        //raycast a cerchio, ritorna il primo oggetto colpito
        //hit = Physics2D.CircleCast(transform.position, sightSee, Vector2.zero, playerMask);
        hit = Physics2D.OverlapCircleAll(transform.position, sightSee, playerMask);

        // If the raycast from enemy to player collide with a player, the enemy will shoot
        for (i = 0; i < hit.Length; i++)
        {
            enemyPlayerHit = Physics2D.Raycast(transform.position, hit[i].transform.position - transform.position, sightSee, enemyMask);
            //Debug.DrawLine(transform.position, hit.transform.position, Color.green);
            //Debug.Log(enemyPlayerHit.collider.name);

            if (enemyPlayerHit.collider != null && enemyPlayerHit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // Build new Vector3 from Enemy to Player. This is used to pass the initial player position in the bullet script
                Debug.DrawLine(transform.position, hit[i].transform.position, Color.red);
                enemyToPlayerVector = enemyPlayerHit.transform.position - new Vector3(transform.position.x, transform.position.y);
                Fire(hit.Length - 1);
            }
            else
                Debug.DrawLine(transform.position, hit[i].transform.position, Color.green);
        }
    }


    public void Fire(int nPlayers)
    {
        // Timer to fire every shootWaitingTime seconds
        if (timer > shootWaitingTime)
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletInstance.GetComponent<Bullet>().RecieveBulletParameter(enemyToPlayerVector);
            if (i == 1 && nPlayers == 1)
                timer = 0;
            else if(i == 0 && nPlayers == 0)
                timer = 0;
        }
    }
}

