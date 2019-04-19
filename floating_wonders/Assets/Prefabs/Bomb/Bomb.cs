using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Bomb : NetworkBehaviour
{
    [SerializeField] private float speed = 0;
    [SerializeField] private int explosionCountdown = 0;
    private Rigidbody2D rb = null;

    void Start()
    {
        StartCoroutine(ExplosionCountdown(explosionCountdown));
    }

    public void AddVelocity(Vector2 direction)
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
        print(rb.velocity);
    }

    //aspetta n secondi, poi la bomba esplode
    private IEnumerator ExplosionCountdown(int secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        NetworkServer.Destroy(this.gameObject);
    }
}
