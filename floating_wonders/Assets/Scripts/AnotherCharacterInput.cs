using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnotherCharacterController))]
public class AnotherCharacterInput : MonoBehaviour
{
    private AnotherCharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<AnotherCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Verse move = Verse.None;

        if (Input.GetKey(KeyCode.A))
            move = Verse.Left;
        else if (Input.GetKey(KeyCode.D))
            move = Verse.Right;


        if (cc.IsClimbing())
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (cc.IsNearTopClimbable())
                {
                    cc.Jump();
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (cc.IsNearBottomClimbable())
                {
                    cc.ReleaseClimbable();
                }
            }

            if (Input.GetKey(KeyCode.W))
                cc.ClimbUp();
            else if (Input.GetKey(KeyCode.S))
                cc.ClimbDown();

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

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (cc.IsDanglingFromEdge())
                {
                    cc.ReleaseEdge();
                }
            }
        }


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
