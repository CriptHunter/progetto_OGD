﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AnotherCharacterController))]
public class AnotherCharacterInput : NetworkBehaviour
{
    public StrikeController strikeController;

    [SerializeField] private SpriteRenderer freccia = null; // per la reference della freccia

    private AnotherCharacterController cc;
    private SpineCharacterAnimator sca;

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
        sca = GetComponent<SpineCharacterAnimator>();
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
                        cc.ReleaseClimbable(true);
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
                            cc.Jump();//ReleaseClimbable();
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
                            cc.Jump();//ReleaseClimbable();
                    }
                }
            }
            else
            {
                // toggle hat
                if (sca != null)
                {
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        if (!cc.IsDanglingFromEdge() && !cc.IsHoldingSomething)
                        {
                            if (sca.HasHat)
                            {
                                sca.SkeletonAnimationOverlay("hat_off", true);
                            }
                            else
                            {
                                sca.SkeletonAnimationOverlay("hat_on", true);
                            }
                        }
                    }
                }

                // jump if possible
                if (Input.GetKeyDown(KeyCode.W))
                {
                    
                    cc.Jump();
                    /*else
                    {
                        if (!cc.IsClimbing())
                            cc.GrabClimbable();
                    }*/
                }

                // if in air and holding W, grab climbable
                if (true)//Input.GetKey(KeyCode.W))
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
                    if (!cc.IsDanglingFromEdge() && !cc.IsClimbing() && !cc.IsHoldingSomething )
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
                if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
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
                if (Input.GetMouseButtonUp(1))
                {
                    if (cc.IsHoldingCharacter())
                    {
                        aimingWithCharacter = false;
                        Vector2 shootDirection;
                        shootDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(cc.GetCharacterHoldingPoint().position);
                        var velocity = Vector2.zero;//cc.GetComponent<Rigidbody2D>().velocity;
                        var applyVelocity = new Vector2(Util.LengthDirX(21f/*13f*/, shootDirection.GetAngle()), Util.LengthDirY(25f, shootDirection.GetAngle())); ;
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
