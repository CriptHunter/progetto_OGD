using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnotherCharacterController : MonoBehaviour
{
    public Text debug;

    public float runSpeed;
    public float runAcceleration;
    public float runFriction;
    public float jumpForce;
    public LayerMask groundLayer;

    private float speed;
    private const float speedThreshold = 0.001f;
    private Verse verse = Verse.Right;
    private new Rigidbody2D rigidbody;
    private new CapsuleCollider2D collider;
    private bool grounded = false;
    private bool running = false;
    private float rundelay = 0;
    private float distToGround;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<CapsuleCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        distToGround = collider.bounds.extents.y;

        speed = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var delta = Time.fixedDeltaTime;

        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.1f;

        RaycastHit2D hit = Physics2D.CapsuleCast(collider.bounds.center, collider.bounds.size, collider.direction, 0, Vector2.down, 0.05f, groundLayer);//Physics2D.Raycast(position, direction, distance, groundLayer);

        //Debug.DrawRay(position, direction, Color.green);
        if (hit.collider != null)
        {
            grounded=true;
        }
        else
        {
            grounded = false;
        }

        if (running)
        {
            if (verse == Verse.Left)
            {
                speed = Mathf.Max(speed - runAcceleration*delta - (speed > 0 ? runFriction * delta : 0), -runSpeed);
            }
            if (verse == Verse.Right)
            {
                speed = Mathf.Min(speed + runAcceleration * delta + (speed < 0 ? runFriction * delta : 0), runSpeed);
            }
        }
        else
        {
            if (speed > speedThreshold)
            {
                speed = speed - runFriction * delta;
                if (speed <= speedThreshold)
                    speed = 0;
            }
            if (speed < -speedThreshold)
            {
                speed = speed + runFriction * delta;
                if (speed >= speedThreshold)
                    speed = 0;
            }
        }
        rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);

        /*if (rigidbody.velocity.x < -speedThreshold)
        {
            speed = Mathf.Max(speed, rigidbody.velocity.x);
        }
        if (rigidbody.velocity.x > speedThreshold)
        {
            speed = Mathf.Min(speed, rigidbody.velocity.x);
        }*/

        /*if (running)
        {
            if (verse == Verse.Left)
            {
                rigidbody.AddForce(new Vector2(-runAcceleration, 0), ForceMode2D.Impulse);
            }
            if (verse == Verse.Right)
            {
                rigidbody.AddForce(new Vector2(runAcceleration, 0), ForceMode2D.Impulse);
            }
        }
        else
        {
            if (rigidbody.velocity.x > speedThreshold)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x - runFriction * delta,rigidbody.velocity.y);
                if (rigidbody.velocity.x <= speedThreshold)
                    rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
            if (rigidbody.velocity.x < -speedThreshold)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x + runFriction * delta, rigidbody.velocity.y);
                if (rigidbody.velocity.x >= speedThreshold)
                    rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
        }

        if (rigidbody.velocity.x > runSpeed)
        {
            rigidbody.velocity = new Vector2(runSpeed, rigidbody.velocity.y);
        }
        if (rigidbody.velocity.x < -runSpeed)
        {
            rigidbody.velocity = new Vector2(-runSpeed, rigidbody.velocity.y);
        }*/

        rundelay -= delta;
        if (rundelay <= 0)
        {
            StopRunning();
        }

        debug.text = "grounded: " + grounded + "\n" +
                    "running: " + running + "\n" +
                    "speed: " + speed + "\n";
    }

    public void Run()
    {
        running = true;
        rundelay = 0.05f;
    }

    private void StopRunning()
    {
        running = false;
    }

    public void Jump()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
        rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    public void Turn(Verse verse)
    {
        this.verse = verse;
    }
}

public enum Verse:int
{
    Left=-1,
    None=0,
    Right=1,
}
