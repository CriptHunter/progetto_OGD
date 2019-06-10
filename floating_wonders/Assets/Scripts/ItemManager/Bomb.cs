using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Bomb : NetworkBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float explosionCountdown;
    [SerializeField] private int radius;
    [SerializeField] private GameObject explosion;

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
    private IEnumerator ExplosionCountdown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameObject explosionObj = (GameObject)Instantiate(explosion, this.transform.position, Quaternion.identity);
        explosionObj.transform.localScale = new Vector3(radius * 2, radius * 2, 1);
        collider.radius = this.radius;
        strikeController.PerformStrike();
        //nascondo la bomba perché è esplosa, così posso distruggere prima l'esplosione e poi la bomba
        this.GetComponent<SpriteRenderer>().enabled = false;
        //aspetto che finisca l'animazione dell'esplosione
        yield return new WaitForSeconds(1f);
        //distruggo bomba e esplosione
        Destroy(explosionObj);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
