using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AnotherCharacterController))]
public class AnotherCharacterInput : NetworkBehaviour
{
    public StrikeController strikeController;

    [SerializeField] private SpriteRenderer freccia = null; // per la reference della freccia

    private AnotherCharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<AnotherCharacterController>();
        if (strikeController == null)
        {
            strikeController = transform.GetChild(0).GetComponent<StrikeController>();
        }
        if (strikeController == null)
        {
            strikeController = gameObject.GetChild("StrikeCollider").GetComponent<StrikeController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer)
        {
            Verse move = Verse.None;
            bool aimingWithCharacter = false;

            // check which direction i want to move to
            if (Input.GetKey(KeyCode.A))
                move = Verse.Left;
            else if (Input.GetKey(KeyCode.D))
                move = Verse.Right;


            if (cc.IsClimbing())
            {
                // if pressing w once, when near the top, jump
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (cc.IsNearTopClimbable())
                    {
                        cc.Jump();
                    }
                }

                // if pressing s once, near the bottom, release
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (cc.IsNearBottomClimbable())
                    {
                        cc.ReleaseClimbable();
                    }
                }

                // climb up and down
                if (Input.GetKey(KeyCode.W))
                    cc.ClimbUp();
                else if (Input.GetKey(KeyCode.S))
                    cc.ClimbDown();

                // switch side/jump away
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (cc.GetVerse() != Verse.Right)
                        cc.Turn(Verse.Right);
                    else
                    {
                        if (Input.GetKey(KeyCode.W))
                            cc.Jump();
                        else
                            cc.ReleaseClimbable();
                    }
                }

                // switch side/jump away
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (cc.GetVerse() != Verse.Left)
                        cc.Turn(Verse.Left);
                    else
                    {
                        if (Input.GetKey(KeyCode.W))
                            cc.Jump();
                        else
                            cc.ReleaseClimbable();
                    }
                }
            }
            else
            {
                // if on ground jump
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (cc.IsTouchingGround() || cc.IsDanglingFromEdge())
                    {
                        cc.Jump();
                    }
                    /*else
                    {
                        if (!cc.IsClimbing())
                            cc.GrabClimbable();
                    }*/
                }

                // if in air and holding W, grab climbable
                if (Input.GetKey(KeyCode.W))
                {
                    if (!cc.IsTouchingGround() && !cc.IsDanglingFromEdge())
                    {
                        if (!cc.IsClimbing())
                            cc.GrabClimbable();
                    }
                }

                // release an eventual edge
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (cc.IsDanglingFromEdge())
                    {
                        cc.ReleaseEdge();
                    }
                }

                // attack
                if (strikeController != null)
                {
                    if (!cc.IsDanglingFromEdge() && !cc.IsClimbing() && !cc.IsHoldingCharacter() )
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            //strikeController.PerformStrike();
                            Cmd_PerformStrike();
                            cc.Attack();
                        }
                    }
                }

                // grab or release character
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (cc.IsHoldingCharacter())
                    {
                        cc.ReleaseCharacter();
                    }
                    else
                    {
                        cc.GrabCharacter();
                    }
                }

                // start aiming
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                {
                    aimingWithCharacter = true;
                }

                // if aiming, rotate pointer toward mouse
                if (freccia != null)
                {
                    freccia.enabled = aimingWithCharacter && cc.IsHoldingCharacter();
                    if (freccia.enabled)
                    {
                        Vector2 shootDirection;
                        shootDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(cc.GetCharacterHoldingPoint().position);
                        freccia.gameObject.transform.eulerAngles = new Vector3(0, 0, shootDirection.GetAngle() + ((cc.GetVerse() == Verse.Right) ? 0f : 180f));
                    }
                }

                // at the end of the aim, throw character to desired direction
                if (Input.GetMouseButtonUp(0))
                {
                    if (cc.IsHoldingCharacter())
                    {
                        aimingWithCharacter = false;
                        Vector2 shootDirection;
                        shootDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(cc.GetCharacterHoldingPoint().position);
                        var velocity = cc.GetComponent<Rigidbody2D>().velocity;
                        var applyVelocity = new Vector2(Util.LengthDirX(13f, shootDirection.GetAngle()), Util.LengthDirY(18f, shootDirection.GetAngle()));
                        var finalVelocity = velocity + applyVelocity;
                        cc.LaunchCharacter(finalVelocity.magnitude, finalVelocity.GetAngle());
                    }
                }

                // reset pointer if unused
                if (!aimingWithCharacter)
                {
                    if (freccia != null)
                    {
                        freccia.gameObject.transform.eulerAngles = Vector3.zero;
                    }
                }
            }

            // run and turn if possible
            if (move != Verse.None)
            {
                if (!cc.IsClimbing())
                {
                    cc.Turn(move);
                    cc.Run();
                }
            }
        }
    }

    [Command] private void Cmd_PerformStrike()
    {
        strikeController.PerformStrike();
    }
}
