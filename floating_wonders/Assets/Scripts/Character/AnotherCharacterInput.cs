﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AnotherCharacterController))]
public class AnotherCharacterInput : NetworkBehaviour
{
    public StrikeController strikeController;

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
                // if on ground jump, if already in air grab any eventual climbable
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (cc.IsTouchingGround() || cc.IsDanglingFromEdge())
                    {
                        cc.Jump();
                    }
                    else
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
                    if (!cc.IsDanglingFromEdge())
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            //strikeController.PerformStrike();
                            Cmd_PerformStrike();
                            cc.Attack();
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Q))
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

                if (Input.GetMouseButtonDown(0))
                {
                    if (cc.IsHoldingCharacter())
                    {
                        Vector2 shootDirection;
                        shootDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(cc.GetCharacterHoldingPoint().position);
                        var velocity = cc.GetComponent<Rigidbody2D>().velocity;
                        var applyVelocity = new Vector2(Util.LengthDirX(13f, shootDirection.GetAngle()), Util.LengthDirY(18f, shootDirection.GetAngle()));
                        var finalVelocity = velocity + applyVelocity;
                        cc.LaunchCharacter(finalVelocity.magnitude, finalVelocity.GetAngle());
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
