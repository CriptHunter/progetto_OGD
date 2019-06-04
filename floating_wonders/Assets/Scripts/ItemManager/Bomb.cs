using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Bomb : NetworkBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int explosionCountdown;
    [SerializeField] private int radius;
    private Rigidbody2D rb = null;
    private CircleCollider2D collider;
    private StrikeController strikeController;

    void Start()
    {
        collider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        strikeController = transform.GetComponentInChildren<StrikeController>();
        StartCoroutine(ExplosionCountdown(explosionCountdown));
    }

    public void AddVelocity(Vector2 direction)
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
    }

    //aspetta n secondi, poi la bomba esplode
    private IEnumerator ExplosionCountdown(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        collider.radius = this.radius;
        strikeController.PerformStrike();
        NetworkServer.Destroy(this.gameObject);
    }
}
