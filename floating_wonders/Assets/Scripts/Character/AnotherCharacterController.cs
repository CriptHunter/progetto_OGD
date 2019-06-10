using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Spine.Unity;

public class AnotherCharacterController : NetworkBehaviour
{
    public Text debug;

    public float runSpeed;
    public float runAcceleration;
    public float runFriction;
    public float jumpForce;
    public float climbSpeed;
    public LayerMask groundLayer;
    public LayerMask climbableLayer;

    private SpineCharacterAnimator animator;

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
    private bool nearCeiling = false;
    //private float groundedDist = 0f;
    private bool running = false;
    private float runDelay = 0;
    private float groundedDelay = 0;
    private float dontStickDelay = 0;

    private CapsuleCollider2D normalCollider; // collider di quando non hai niente
    public CapsuleCollider2D characterHoldingCollider; // collider di quando hai un compagno sopra di te
    public CircleCollider2D characterGrabCollider; // collider usato per raccogliere i persobnaggi
    private AnotherCharacterController potentialCharacter; // potenziale character che può raccogliere
    private AnotherCharacterController heldCharacter = null;
    private AnotherCharacterController isHeldBy = null;
    private Transform characterHoldingPoint;

    public Transform GetCharacterHoldingPoint()
    {
        return characterHoldingPoint;
    }

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
                ReleaseClimbable();
            }
            else
            {
                PhysicsActive = true;
            }
        }
    }

    public void Activate(bool keepMomentum)
    {
        float len = 0, dir = 0;
        if (keepMomentum)
        {
            dir = rigidbody.velocity.GetAngle();
            print("angle is " + dir);
            len = rigidbody.velocity.magnitude;
        }
        Active = true;

        if (keepMomentum)
        {
            ApplyImpulse(dir, len);
        }
    }

    public void Deactivate(bool keepMomentum)
    {
        Vector2 vel = Vector2.zero;
        if (keepMomentum)
        {
            vel = rigidbody.velocity;
        }
        Active = false;
        if (keepMomentum)
        {
            rigidbody.velocity = vel;
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
    private Verse targetEdgeVerse = Verse.None; // verso dal quale stai aggrappato
    private float dontGrabEdgeDelay = 0; // ritardo entro il quale non può richiappare un edge

    private Climbable potentialClimbable; // se sei vicino a un climbable, reference del climbable altrimenti null
    private Climbable targetClimbable; // se è attaccato a un climbable gameobject, altrimenti null
    private bool climbUpRequest = false; // se è stata fatta richiesta di arrampicarsi su o giù
    private bool climbDownRequest = false;
    private Verse climbVerse = Verse.None;
    private float dontGrabClimbableDelay = 0; // ritardo entro il quale non può richiappare un climbable

    void Awake()
    {
        collider = GetComponents<CapsuleCollider2D>()[0]; // the first collider is the good one
        normalCollider = collider;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<SpineCharacterAnimator>();
        if (animator != null)
        {
            animator.DontReloadSameAnimation = true;
        }

        var egc = gameObject.GetChild("EdgeGrabCollider");
        if (egc != null)
        {
            edgeGrabCollider = egc.GetComponent<CharacterEdgeGrab>();
        }

        characterHoldingPoint = gameObject.GetChild("CharacterHoldingPoint").transform;

        speed = 0;
        initialGravityScale = rigidbody.gravityScale;
    }

    public void Animate()
    {
        if (animator != null)
        {
            if (IsBeingHeldByCharacter())
            {
                animator.Animation = "grabbed";
            }
            else
            {
                if (IsClimbing())
                {
                    if (climbVerse == Verse.Up)
                    {
                        animator.Animation = "climb_up";
                    }
                    else
                    {
                        if (climbVerse == Verse.Down)
                        {
                            animator.Animation = "climb_down";
                        }
                        else
                        {
                            animator.Animation = "climb";
                        }
                    }
                }
                else
                {
                    if (IsDanglingFromEdge())
                    {
                        animator.Animation = "hanging_edge";
                    }
                    else
                    {
                        string suffix = "";
                        if (IsHoldingCharacter())
                        {
                            suffix = "_holdingcharacter";
                        }

                        if (IsTouchingGround())
                        {
                            if (IsRunning())
                            {
                                animator.Animation = "run" + suffix;
                            }
                            else
                            {
                                animator.Animation = "stand" + suffix;
                            }
                        }
                        else
                        {
                            if (IsMovingUpward())
                            {
                                animator.Animation = "jump_up" + suffix;
                            }
                            else
                            {
                                animator.Animation = "jump_down" + suffix;
                            }
                        }
                    }
                }
            }
        }
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

        climbVerse = Verse.None;

        if (Active)
        {
            if (!IsClimbing())
            {
                var edg = edgeGrabCollider.GetEdge();
                if (edg != null && !IsDanglingFromEdge() && dontGrabEdgeDelay < speedThreshold)
                {
                    if (rigidbody.velocity.y < 0.2f)
                    {
                        GrabEdge(edg);
                    }
                }

                if (IsDanglingFromEdge())
                {
                    StickToEdge(targetEdge);
                }
            }
            else
            {
                if (!climbUpRequest && !climbDownRequest)
                {
                    rigidbody.velocity = Vector2.zero;
                }
                else
                {
                    if (climbUpRequest)
                    {
                        //rigidbody.position = new Vector2(rigidbody.position.x, rigidbody.position.y + climbSpeed/50f);
                        rigidbody.velocity = Vector2.up * climbSpeed;
                        climbUpRequest = false;
                        climbVerse = Verse.Up;
                    }
                    if (climbDownRequest)
                    {
                        //rigidbody.position = new Vector2(rigidbody.position.x, rigidbody.position.y - climbSpeed/50f);
                        rigidbody.velocity = Vector2.down * climbSpeed;
                        climbDownRequest = false;
                        climbVerse = Verse.Down;
                    }
                }
            }
        }

        CalculateProximity();

        if (IsClimbing())
        {
            StickToClimbable(targetClimbable.gameObject);
            if (grounded && !nearCeiling)
            {
                ReleaseClimbable();
            }
        }

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
        dontGrabClimbableDelay = Mathf.Max(0, dontGrabClimbableDelay - delta);

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

        Animate();

        RaycastHit2D hit = Physics2D.CapsuleCast(collider.bounds.center, collider.bounds.size, collider.direction, 0, verse.Vector(), 0.99f, groundLayer);
        bool spazio = hit.collider == null;
        debug.text = "grounded: " + grounded + "\n" +
                    "ceiling: " + nearCeiling + "\n" +
                    "spazio di fronte: " + spazio + "\n" +
                    "dont stick: " + dontStickDelay + "\n" +
                    "wall left: " + nearWallLeft + "\n" +
                    "wall right: " + nearWallRight + "\n" +
                    "potential character: " + potentialCharacter + "\n" +
                    "TEST: " + (potentialCharacter==this) + "\n";
        /*"running: " + running + "\n" +
        "speed: " + speed + "\n" +
        "climbing: " + targetClimbable + "\n" +
        "dangling: " + IsDanglingFromEdge() + "\n";*/
    }



    private void LateUpdate()
    {
        if (IsHoldingCharacter())
        {
            heldCharacter.rigidbody.position = characterHoldingPoint.position;
            heldCharacter.TurnInternal(verse,false);
            if (!heldCharacter.Active)
            {
                Animate();
            }
        }
        if (IsBeingHeldByCharacter())
        {
            rigidbody.position = isHeldBy.GetCharacterHoldingPoint().position;
            TurnInternal(isHeldBy.transform.localScale.x>0?Verse.Right:Verse.Left,false); // per qualche motivo leggere isHeldBy.verse torna sempre Up?
        }
    }

    /// <summary>
    /// Call this method every frame and the character will run/move toward the direction it is currently facing.
    /// Don't call this method and the player will stop running/moving.
    /// </summary>
    public void Run()
    {
        if (Active && !IsDanglingFromEdge() && !IsClimbing() && !IsBeingHeldByCharacter())
        {
            running = true;
            runDelay = 0.05f;
        }
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
            if (IsClimbing())
            {
                ReleaseClimbable();
                Turn(verse.Opposite());
                //VerticalImpulse(jumpForce*10);
                if (verse == Verse.Left)
                    ApplyImpulse(90 + 25f, 17f);
                else
                    ApplyImpulse(90 - 25f, 17f);
                //ApplyImpulse(verse.Angle(), 10f);
            }
            else
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
                        if (IsHoldingCharacter())
                        {
                            VerticalImpulse(jumpForce);// * 0.85f);
                        }
                        else
                        {
                            if (!IsBeingHeldByCharacter())
                                VerticalImpulse(jumpForce);
                        }
                    }
                }
            }
        }
    }

    private void VerticalImpulse(float strength)
    {
        if (Active && PhysicsActive && !IsBeingHeldByCharacter())
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
    /// Commands the character to turn to the specified direction. (The direction it will look at)
    /// </summary>
    /// <param name="verse">Direction to turn to</param>
    /// <returns>false if for some reason you couldn't turn, true otherwise</returns>
    public bool Turn(Verse verse)
    {
        if (!IsBeingHeldByCharacter())
        {
            return TurnInternal(verse);
        }
        else
        {
            return false;
        }
    }

    private bool TurnInternal(Verse verse, bool alsoBroadcastCommand=true)
    {
        if (!IsClimbing())
        {
            this.verse = verse;
            if (alsoBroadcastCommand)
                Cmd_SetLocalScale(verse);
            else
                transform.localScale = new Vector3((int)verse, 1, 1);
            if (IsDanglingFromEdge() && targetEdgeVerse != verse)
            {
                ReleaseEdge();
            }
            return true;
        }
        else
        {
            if (verse != this.verse)
            {
                // girati solo se c'è spazio
                // per qualche motivo ci va l'opposite
                RaycastHit2D hit = Physics2D.CapsuleCast(collider.bounds.center, collider.bounds.size, collider.direction, 0, verse.Opposite().Vector(), 0.99f, groundLayer);
                //Debug.Log("provo a girarmi, il collider è " + hit.collider);
                if (hit.collider == null)
                {
                    this.verse = verse;
                    if (alsoBroadcastCommand)
                        Cmd_SetLocalScale(verse);
                    else
                        transform.localScale = new Vector3((int)verse, 1, 1);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }

    [Command]
    private void Cmd_SetLocalScale(Verse verse)
    {
        Rpc_SetLocalScale(verse);
    }

    [ClientRpc]
    private void Rpc_SetLocalScale(Verse verse)
    {
        this.transform.localScale = new Vector3((int)verse, 1, 1);
    }

    private void CalculateProximity()
    {
        RaycastHit2D hit;

        hit = Physics2D.CapsuleCast((Vector2)collider.bounds.center, collider.bounds.size, collider.direction, 0, Vector2.down, 0.1f, groundLayer);
        if (hit.collider != null)
            grounded = true;
        else
            grounded = false;

        hit = Physics2D.CapsuleCast((Vector2)collider.bounds.center, collider.bounds.size, collider.direction, 0, Vector2.up, 0.1f, groundLayer);
        if (hit.collider != null)
            nearCeiling = true;
        else
            nearCeiling = false;

        hit = Physics2D.CapsuleCast((Vector2)collider.bounds.center, collider.bounds.size - new Vector3(0, 0.1f, 0), collider.direction, 0, new Vector2(1, 2), 0.05f, groundLayer);
        if (hit.collider != null)
            nearWallRight = true;
        else
            nearWallRight = false;

        hit = Physics2D.CapsuleCast((Vector2)collider.bounds.center, collider.bounds.size - new Vector3(0, 0.1f, 0), collider.direction, 0, new Vector2(-1, 2), 0.05f, groundLayer);
        if (hit.collider != null)
            nearWallLeft = true;
        else
            nearWallLeft = false;

        hit = Physics2D.CapsuleCast((Vector2)collider.bounds.center, collider.bounds.size - new Vector3(0, 0.1f, 0), collider.direction, 0, Vector2.one, 0.0f, climbableLayer);
        if (hit.collider != null)
            potentialClimbable = hit.collider.gameObject.GetComponent<Climbable>();
        else
            potentialClimbable = null;

        potentialCharacter = null;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(characterGrabCollider.bounds.center, characterGrabCollider.radius, Vector2.right, 0.0f);
        for (int i = 0; i < hits.Length; i++)
        {
            AnotherCharacterController other = hits[i].collider.gameObject.GetComponent<AnotherCharacterController>();
            if (other != null && other != this)
            {
                potentialCharacter = other;
            }
        }
    }

    public bool IsDanglingFromEdge()
    {
        return targetEdge != null;
    }

    private void GrabEdge(GameObject edge)
    {
        if (Active)
        {
            if (!IsClimbing() && !IsHoldingCharacter() && !IsBeingHeldByCharacter() && dontGrabEdgeDelay<=speedThreshold)
            {
                if (!IsDanglingFromEdge())
                {
                    Turn(edge.GetComponent<EdgeProperties>().EdgeVerse);
                    targetEdgeVerse = verse;

                    targetEdge = edge;
                    PhysicsActive = false;
                    //StickToEdge(edge);
                    rigidbody.velocity = Vector2.zero;
                    ReleaseCharacter();
                }
            }
        }
    }

    private void StickToEdge(GameObject edge)
    {
        if (Active)
            rigidbody.position = edge.transform.position - (new Vector3(edgeGrabCollider.gameObject.transform.localPosition.x * (int)targetEdge.GetComponent<EdgeProperties>().EdgeVerse, edgeGrabCollider.gameObject.transform.localPosition.y, edgeGrabCollider.gameObject.transform.localPosition.z));
    }

    public void ReleaseEdge()
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

    public bool GrabClimbable()
    {
        if (Active)
        {
            if (potentialClimbable != null && !IsTouchingGround() && !IsHoldingCharacter() && !IsBeingHeldByCharacter() && dontGrabClimbableDelay<=speedThreshold)
            {
                ReleaseEdge();
                ReleaseCharacter();
                PhysicsActive = false;
                targetClimbable = potentialClimbable;
                if (rigidbody.position.x > targetClimbable.gameObject.transform.position.x)
                {
                    Turn(Verse.Left);
                }
                else
                {
                    Turn(Verse.Right);
                }
                StickToClimbable(targetClimbable.gameObject);
                rigidbody.velocity = Vector2.zero;
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public void ClimbUp()
    {
        if (IsClimbing())
            climbUpRequest = true;
    }

    public void ClimbDown()
    {
        if (IsClimbing())
            climbDownRequest = true;
    }

    private void StickToClimbable(GameObject climbable)
    {
        if (Active)
        {
            BoxCollider2D box = climbable.GetComponent<BoxCollider2D>();
            rigidbody.position = new Vector2(climbable.transform.position.x - 0.5f * (int)verse, Mathf.Clamp(rigidbody.position.y, box.bounds.center.y - box.bounds.extents.y + 0.9f, box.bounds.center.y + box.bounds.extents.y - 0.9f));
        }
    }

    public bool IsNearTopClimbable()
    {
        if (IsClimbing())
        {
            BoxCollider2D box = targetClimbable.GetComponent<BoxCollider2D>();
            return (rigidbody.position.y > box.bounds.center.y + box.bounds.extents.y - 0.2f - 0.9f);
        }
        else return false;
    }

    public bool IsNearBottomClimbable()
    {
        if (IsClimbing())
        {
            BoxCollider2D box = targetClimbable.GetComponent<BoxCollider2D>();
            return (rigidbody.position.y < box.bounds.center.y - box.bounds.extents.y + 0.2f + 0.9f);
        }
        else return false;
    }

    public void ReleaseClimbable()
    {
        if (IsClimbing())
        {
            if (Active)
            {
                PhysicsActive = true;
            }

            dontGrabClimbableDelay = 0.25f;
            targetClimbable = null;
            climbUpRequest = false;
            climbDownRequest = false;
        }
    }

    public bool IsClimbing()
    {
        return targetClimbable != null;
    }

    private void CalculateSpeed(float delta)
    {
        if (Active)
        {
            if (!IsClimbing())
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
            if (!(speed > -speedThreshold && speed < speedThreshold))
            {
                //rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
                MoveToSolid(270, 0.5f);
            }
        }
    }

    private bool MoveToSolid(float direction, float maxDist)
    {
        var dir = Util.DegreeToVector2(direction);
        RaycastHit2D hit = Physics2D.CapsuleCast((Vector2)collider.bounds.center - collider.offset, collider.bounds.size, collider.direction, 0, dir, maxDist, groundLayer); ;
        if (hit.collider != null)
        {
            rigidbody.position = hit.centroid;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanGrabCharacter(bool checkForCharacter=true)
    {
        var collider = characterHoldingCollider;
        if (collider != null)
        {
            RaycastHit2D hit = Physics2D.CapsuleCast((Vector2)collider.bounds.center, collider.bounds.size, collider.direction, 0, Vector2.up, 0.1f, groundLayer);
            if (hit.collider != null)
                return false;
            else
            {
                if (checkForCharacter)
                {
                    if (potentialCharacter == null)
                        return false;
                    else
                        return true;
                }
                else
                {
                    return true;
                }
            }
        }
        else
        {
            return false;
        }
    }

    public void GrabCharacter()
    {
        if (!IsDanglingFromEdge() && !IsClimbing() && !IsHoldingCharacter() && !IsBeingHeldByCharacter())
        {
            if (potentialCharacter != null)
            {
                CmdActionGrab(gameObject, potentialCharacter.gameObject);
            }
        }
    }

    public bool IsHoldingCharacter()
    {
        return heldCharacter != null;
    }

    public void ReleaseCharacter()
    {
        CmdActionUngrab(gameObject);
    }

    [Command]
    private void CmdActionGrab(GameObject grabber, GameObject grabbed)
    {
        RpcActionGrab(grabber, grabbed);
    }
    [ClientRpc]
    private void RpcActionGrab(GameObject grabber, GameObject grabbed)
    {
        AnotherCharacterController actor, acted;
        actor = grabber.GetComponent<AnotherCharacterController>();
        acted = grabbed.GetComponent<AnotherCharacterController>();

        if (actor!=null && acted!=null)
        {
            if (actor.CanGrabCharacter(false))
            {
                if (!actor.IsDanglingFromEdge() && !actor.IsClimbing() && !actor.IsHoldingCharacter())
                {
                    acted.ReleaseCharacter();
                    acted.ReleaseClimbable();
                    acted.ReleaseEdge();
                    acted.isHeldBy = actor;
                    acted.Active = false;
                    acted.DisableColliders();
                    acted.GetComponent<NetworkTransform>().enabled = false;
                    //acted.gameObject.transform.parent = actor.GetCharacterHoldingPoint();
                    //acted.gameObject.transform.localPosition = Vector3.zero;

                    actor.heldCharacter = acted;
                    actor.normalCollider.isTrigger = true;
                    actor.characterHoldingCollider.isTrigger = false;
                    actor.collider = actor.characterHoldingCollider;

                    acted.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 2;
                    actor.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 1;
                }
            }
        }
    }

    [Command]
    private void CmdActionUngrab(GameObject grabber)
    {
        RpcActionUngrab(grabber);
    }
    [ClientRpc]
    private void RpcActionUngrab(GameObject grabber)
    {
        Ungrab(grabber);
    }

    private void Ungrab(GameObject grabber)
    {
        AnotherCharacterController actor, acted;
        actor = grabber.GetComponent<AnotherCharacterController>();

        if (actor != null)
        {
            if (actor.IsHoldingCharacter())
            {
                acted = actor.heldCharacter;
                acted.isHeldBy = null;
                acted.Active = true;
                acted.EnableColliders();
                acted.GetComponent<NetworkTransform>().enabled = true;
                //acted.gameObject.transform.parent = null;

                actor.heldCharacter = null;
                actor.normalCollider.isTrigger = false;
                actor.characterHoldingCollider.isTrigger = true;
                actor.collider = actor.normalCollider;


                acted.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 1;
                actor.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 1;
            }
        }
    }

    public bool IsBeingHeldByCharacter()
    {
        return isHeldBy != null;
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
            print("Apply impulse: " + direction + " " + strength);
            impulseHSpeed = Util.LengthDirX(strength, direction);
            VerticalImpulse(Util.LengthDirY(strength, direction));
        }
    }

    public void LaunchCharacter(float force, float direction)
    {
        if (IsHoldingCharacter())
        {
            CmdActionLaunchCharacter(gameObject, heldCharacter.gameObject,  force, direction);
        }
    }

    [Command]
    private void CmdActionLaunchCharacter(GameObject launcher, GameObject launched, float force, float direction)
    {
        RpcActionLaunchCharacter(launcher, launched, force, direction);
    }
    [ClientRpc]
    private void RpcActionLaunchCharacter(GameObject launcher, GameObject launched, float force, float direction)
    {
        AnotherCharacterController actor = launcher.GetComponent<AnotherCharacterController>();
        AnotherCharacterController acted = launched.GetComponent<AnotherCharacterController>();
        if (actor.IsHoldingCharacter())
        {
            actor.Ungrab(launcher);
            acted.ApplyImpulse(direction, force);
        }
    }

    public void DisableColliders()
    {
        normalCollider.enabled = false;
        characterHoldingCollider.enabled = false;
    }
    public void EnableColliders()
    {
        normalCollider.enabled = true;
        characterHoldingCollider.enabled = true;
    }

    public void Attack()
    {
        if (!IsDanglingFromEdge() && !IsClimbing() && !IsHoldingCharacter() && !IsBeingHeldByCharacter())
        {
            if (animator != null)
            {
                animator.SkeletonAnimationOverlay("attack0", true);
            }
        }
    }

    public Verse GetVerse()
    {
        return verse;
    }

    public bool IsTouchingGround()
    {
        return grounded;
    }

    public bool IsRunning()
    {
        return running;
    }

    public bool IsMovingUpward()
    {
        return rigidbody.velocity.y > 0;
    }
    public bool IsMovingDownward()
    {
        return rigidbody.velocity.y < 0;
    }
}
