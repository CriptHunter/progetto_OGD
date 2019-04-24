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

    private CharacterEdgeGrab edgeGrabCollider;
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

    private bool active = true;
    /// <summary>
    /// Activates or deactivates the character controller. When deactivated, the movement of this gameobject will be driven by normal physics or
    /// can be manipulated by other scripts. Deactivating the character controller will also release any eventual edge.
    /// Keep in mind that deactivating the character controller will also deactivate its gravity. The status
    /// of the character controller will be resumed when Active is set to true again.
    /// </summary>
    public bool Active
    {
        get
        {
            return active;
        }
        set
        {
            active = value;
            if (!active)
            {
                PhysicsActive = false;
                ReleaseEdge();
            }
            else
            {
                PhysicsActive = true;
            }
        }
    }

    private bool physicsActive = true; // se la fisica normale del movimento è attiva
    private bool PhysicsActive
    {
        get
        {
            return physicsActive;
        }
        set
        {
            physicsActive = value;
            if (!physicsActive)
            {
                rigidbody.gravityScale = 0;
                speed = 0;
                impulseHSpeed = 0;
            }
            else
            {
                rigidbody.gravityScale = initialGravityScale;
            }
        }
    }
    private GameObject targetEdge; // se è attaccato a un edge gameobject, altrimenti null
    private Verse targetEdgeVerse= Verse.None; // verso dal quale stai aggrappato
    private float dontGrabEdgeDelay = 0; // ritardo entro il quale non può richiappare un edge

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<CapsuleCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        var egc = gameObject.GetChild("EdgeGrabCollider");
        if (egc != null)
        {
            edgeGrabCollider = egc.GetComponent<CharacterEdgeGrab>();
        }

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

        if (Active)
        {
            var edg = edgeGrabCollider.GetEdge();
            if (edg != null && !IsDanglingFromEdge() && dontGrabEdgeDelay < speedThreshold)
            {
                if (rigidbody.velocity.y < 0.2f)
                    GrabEdge(edg);
            }

            if (IsDanglingFromEdge())
            {
                StickToEdge(targetEdge);
            }
        }

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
        dontGrabEdgeDelay = Mathf.Max(0, dontGrabEdgeDelay - delta);
        if (Active)
        {
            StickToGround();
        }

        if (PhysicsActive && Active)
        {
            rigidbody.velocity = new Vector2(speed + impulseHSpeed, rigidbody.velocity.y);
        }

        if (impulseHSpeed < -speedThreshold)
        {
            impulseHSpeed = Mathf.Min(impulseHSpeed + impulseHfriction, 0);
        }
        if (impulseHSpeed > speedThreshold)
        {
            impulseHSpeed = Mathf.Max(impulseHSpeed - impulseHfriction, 0);
        }

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
                    "speed: " + speed + "\n" +
                    "dangling: " + IsDanglingFromEdge() + "\n";
    }

    /// <summary>
    /// Call this method every frame and the character will run/move toward the direction it is currently facing.
    /// Don't call this method and the player will stop running/moving.
    /// </summary>
    public void Run()
    {
        running = true;
        runDelay = 0.05f;
    }

    private void StopRunning()
    {
        running = false;
    }

    /// <summary>
    /// Commands the character to jump, if possible.
    /// </summary>
    public void Jump()
    {
        if (Active)
        {
            if (IsDanglingFromEdge())
            {
                ReleaseEdge();
                VerticalImpulse(jumpForce);
            }
            else
            {
                if (grounded)
                {
                    VerticalImpulse(jumpForce);
                }
            }
        }
    }

    private void VerticalImpulse(float strength)
    {
        if (Active && PhysicsActive)
        {
            if (strength > 0)
            {
                groundedDelay = 0;
                dontStickDelay = 0.25f;
            }
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            rigidbody.AddForce(new Vector2(0, strength), ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Commands the character to turn to the specified direction.
    /// </summary>
    /// <param name="verse">Direction to turn to</param>
    public void Turn(Verse verse)
    {
        this.verse = verse;
        transform.localScale = new Vector3((int)verse, 1, 1);
        if (IsDanglingFromEdge() && targetEdgeVerse != verse)
        {
            ReleaseEdge();
        }
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

    public bool IsDanglingFromEdge()
    {
        return targetEdge != null;
    }

    private void GrabEdge(GameObject edge)
    {
        if (Active)
        {
            if (!IsDanglingFromEdge())
            {
                Turn(edge.GetComponent<EdgeProperties>().EdgeVerse);
                targetEdgeVerse = verse;

                targetEdge = edge;
                PhysicsActive = false;
                //StickToEdge(edge);
                rigidbody.velocity = Vector2.zero;
            }
        }
    }

    private void StickToEdge(GameObject edge)
    {
        if (Active)
            rigidbody.position = edge.transform.position - (new Vector3(edgeGrabCollider.gameObject.transform.localPosition.x * (int)targetEdge.GetComponent<EdgeProperties>().EdgeVerse, edgeGrabCollider.gameObject.transform.localPosition.y, edgeGrabCollider.gameObject.transform.localPosition.z));
    }

    private void ReleaseEdge()
    {
        if (IsDanglingFromEdge())
        {
            if (Active)
            {
                PhysicsActive = true;
                dontGrabEdgeDelay = 0.35f;
            }
            targetEdge = null;
        }
    }

    private void CalculateSpeed(float delta)
    {
        if (Active)
        {
            if (!IsDanglingFromEdge())
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

                if (nearWallLeft && speed + impulseHSpeed < -speedThreshold)
                {
                    speed = 0;
                    impulseHSpeed = 0;
                }

                if (nearWallRight && speed + impulseHSpeed > speedThreshold)
                {
                    speed = 0;
                    impulseHSpeed = 0;
                }
            }
            else
            {
                speed = 0;
                impulseHSpeed = 0;
            }
        }
        else
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
        if (hit.collider != null)
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

    /// <summary>
    /// Applies an impulse to the character. Useful for a knockback.
    /// </summary>
    /// <param name="direction">Direction of the impulse.</param>
    /// <param name="strength">Strength of the impulse.</param>
    public void ApplyImpulse(float direction, float strength)
    {
        if (Active && PhysicsActive)
        {
            speed = 0;
            impulseHSpeed = Util.LengthDirX(strength, direction);
            VerticalImpulse(Util.LengthDirY(strength, direction));
        }
    }

    public Verse GetVerse()
    {
        return verse;
    }

}

/// <summary>
/// List of possible directions for the character.
/// </summary>
public enum Verse:int
{
    Left=-1,
    None=0,
    Right=1,
}
