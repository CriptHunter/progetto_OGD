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
        //se il giocatore vuole saltare
        bool jump = false;

        if (Input.GetKey(KeyCode.A))
            move = Verse.Left;
        else if (Input.GetKey(KeyCode.D))
            move = Verse.Right;
        if (Input.GetKeyDown(KeyCode.W))
            jump = true;
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (cc.Active)
            {
                cc.Deactivate(true);
            }
            else
            {
                cc.Activate(true);
            }
        }

        if (move != Verse.None)
        {
            cc.Turn(move);
            cc.Run();
        }

        if (jump)
        {
            cc.Jump();
        }
    }
}
