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
    public void addVelocity(Vector2 direction)
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
    }

    //aspetta n secondi, poi la bomba esplode
    private IEnumerator ExplosionCountdown(int secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        Cmd_Destroy(this.gameObject);
    }

    [Command]
    public void Cmd_Destroy(GameObject obj)
    {
        NetworkServer.Destroy(obj);
    }
}
