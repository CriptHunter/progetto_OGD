using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pad : MonoBehaviour
{
    private bool padPressed;

    public void SetPadPressed (bool padPressed)
    {
        padPressed = this.padPressed;
    }

    public bool GetPadPressed()
    {
        return padPressed;
    }
    
    void Start()
    {
        padPressed = false;
        //padArray.AddPad(this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        padPressed = true;
        //padArray.InPadCollision(this);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        padPressed = false;
        //padArray.OutPadCollision(this);
    }
}
