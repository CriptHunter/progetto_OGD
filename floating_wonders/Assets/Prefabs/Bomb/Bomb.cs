using UnityEngine;
using UnityEngine.Networking;

public class Bomb : NetworkBehaviour
{
    [SerializeField] private float speed = 2;
    private Rigidbody2D rb = null;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        print((transform.right + transform.up)*speed);
        rb.velocity = (transform.right + transform.up) * speed;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
