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

    private float initialGravityScale;
    private float speed;
    private float impulseHSpeed;
    private float impulseHfriction = 0.5f;
    private const float speedThreshold = 0.001f;
    private Verse verse = Verse.Right;
    private new Rigidbody2D rigidbody;
    private new CapsuleCollider2D collider;
    private bool nearWallLeft = false;
    private bool nearWallRight = false;
    private bool grounded = false;
    //private float groundedDist = 0f;
    private bool running = false;
    private float runDelay = 0;
    private float groundedDelay = 0;
    private float dontStickDelay = 0;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<CapsuleCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        speed = 0;
        initialGravityScale = rigidbody.gravityScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var delta = Time.fixedDeltaTime;
        /*var dir = Util.DegreeToVector2(270);
        RaycastHit2D hit = Physics2D.CapsuleCast(collider.bounds.center, collider.bounds.size, collider.direction, 0, dir, maxDist, groundLayer);
        if (hit.collider != null)
        {
            groundedDist = transform.position.y - hit.centroid.y;
        }
        else
        {
            groundedDist = float.MaxValue;
        }*/

        CalculateProximity();
        CalculateSpeed(delta);

        if (!grounded)
        {
            //rigidbody.gravityScale = initialGravityScale;
            groundedDelay = Mathf.Max(0, groundedDelay - delta);
            //dontStickDelay = 0.25f;
        }
        else
        {
            groundedDelay = 0.1f;
            //MoveToSolid(270, 1.5f);
        }
        dontStickDelay = Mathf.Max(0, dontStickDelay - delta);
        StickToGround();
        
        rigidbody.velocity = new Vector2(speed+impulseHSpeed, rigidbody.velocity.y);

        if (impulseHSpeed < -speedThreshold)
        {
            impulseHSpeed = Mathf.Min(impulseHSpeed + impulseHfriction, 0);
        }
        if (impulseHSpeed > speedThreshold)
        {
            impulseHSpeed = Mathf.Max(impulseHSpeed - impulseHfriction, 0);
        }
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

        runDelay -= delta;
        if (runDelay <= 0)
        {
            StopRunning();
        }

        debug.text = "grounded: " + grounded + "\n" +
                    "dont stick: " + dontStickDelay + "\n" +
                    "wall left: " + nearWallLeft + "\n" +
                    "wall right: " + nearWallRight + "\n" +
                    "running: " + running + "\n" +
                    "speed: " + speed + "\n";
    }

    public void Run()
    {
        running = true;
        runDelay = 0.05f;
    }

    private void StopRunning()
    {
        running = false;
    }

    public void Jump()
    {
        if (grounded)
        {
            VerticalImpulse(jumpForce);
        }
    }

    private void VerticalImpulse(float strength)
    {
        if (strength > 0)
        {
            groundedDelay = 0;
            dontStickDelay = 0.25f;
        }
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
        rigidbody.AddForce(new Vector2(0, strength), ForceMode2D.Impulse);
    }

    public void Turn(Verse verse)
    {
        this.verse = verse;
    }

    private void CalculateProximity()
    {
        RaycastHit2D hit = Physics2D.CapsuleCast(collider.bounds.center, collider.bounds.size, collider.direction, 0, Vector2.down, 0.1f, groundLayer);
        if (hit.collider != null)
            grounded = true;
        else
            grounded = false;

        hit = Physics2D.CapsuleCast(collider.bounds.center, collider.bounds.size - new Vector3(0, 0.1f, 0), collider.direction, 0, new Vector2(1, 2), 0.05f, groundLayer);
        if (hit.collider != null)
            nearWallRight = true;
        else
            nearWallRight = false;

        hit = Physics2D.CapsuleCast(collider.bounds.center, collider.bounds.size - new Vector3(0, 0.1f, 0), collider.direction, 0, new Vector2(-1, 2), 0.05f, groundLayer);
        if (hit.collider != null)
            nearWallLeft = true;
        else
            nearWallLeft = false;
    }

    private void CalculateSpeed(float delta)
    {
        if (running)
        {
            if (verse == Verse.Left)
            {
                speed = Mathf.Max(speed - runAcceleration * delta - (speed > 0 ? runFriction * delta : 0), -runSpeed);
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

        if (nearWallLeft && speed+impulseHSpeed<-speedThreshold)
        {
            speed = 0;
            impulseHSpeed = 0;
        }

        if (nearWallRight && speed+impulseHSpeed > speedThreshold)
        {
            speed = 0;
            impulseHSpeed = 0;
        }
    }

    private void StickToGround()
    {
        if (/*groundedDelay > speedThreshold &&*/ dontStickDelay < speedThreshold)
        {
            //if (!(speed>-speedThreshold && speed<speedThreshold))
            {
                //rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                MoveToSolid(270, 0.5f);
            }
        }
    }

    private bool MoveToSolid(float direction, float maxDist)
    {
        var dir = Util.DegreeToVector2(direction);
        RaycastHit2D hit = Physics2D.CapsuleCast(collider.bounds.center, collider.bounds.size, collider.direction, 0, dir, maxDist, groundLayer);
        if (hit.collider!=null)
        {
            //transform.position = hit.centroid;
            rigidbody.position = hit.centroid;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ApplyImpulse(float direction, float strength)
    {
        speed = 0;
        impulseHSpeed = Util.LengthDirX(strength, direction);
        VerticalImpulse(Util.LengthDirY(strength, direction));
    }
}

public enum Verse:int
{
    Left=-1,
    None=0,
    Right=1,
}
