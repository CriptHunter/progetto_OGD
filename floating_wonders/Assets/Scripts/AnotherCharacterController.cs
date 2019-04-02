using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherCharacterController : MonoBehaviour
{
    public GameObject debug;

    public float runSpeed;
    public float jumpForce;
    public LayerMask groundLayer;

    private Verse verse = Verse.Right;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private bool grounded = false;
    private float distToGround;
    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        distToGround = collider.bounds.extents.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Verse move = Verse.None;
        //se il giocatore vuole saltare
        bool jump = false;

        if (Input.GetKey(KeyCode.A))
            move = Verse.Left;
        else if (Input.GetKey(KeyCode.D))
            move = Verse.Right;
        if (Input.GetKeyDown(KeyCode.W))
            jump = true;

        if (move != Verse.None)
        {
            Turn(move);
            Run();
        }

        if (jump)
        {
            Jump();
        }

        grounded = IsGrounded();
        debug.SetActive(grounded);
        Debug.Log(grounded);
    }

    public void Run()
    {
        rigidbody.velocity = new Vector2(runSpeed * (float)verse, rigidbody.velocity.y);
    }

    public void Jump()
    {
        rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    public void Turn(Verse verse)
    {
        this.verse = verse;
    }

    private bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.1f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);

        //Debug.DrawRay(position, direction, Color.green);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }
}

public enum Verse:int
{
    Left=-1,
    None=0,
    Right=1,
}
