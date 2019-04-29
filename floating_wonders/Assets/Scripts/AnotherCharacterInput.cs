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

        if (Input.GetKey(KeyCode.W))
            cc.ClimbUp();
        else if (Input.GetKey(KeyCode.S))
            cc.ClimbDown();


        if (move != Verse.None)
        {
            cc.Turn(move);
            cc.Run();
        }
    }
}
