using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pad : MonoBehaviour
{
    private bool padPressed;
    private List<GameObject> playerOnPad;
    private LayerMask playerMask;

    public void SetPadPressed(bool padPressed)
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
        playerOnPad = new List<GameObject>();
        playerMask = LayerMask.NameToLayer("Player");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        padPressed = true;
        playerOnPad.Add(collision.gameObject);

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (playerOnPad.Contains(collision.gameObject))
        {
            playerOnPad.Remove(collision.gameObject);
            if (playerOnPad.Count == 0)
                padPressed = false;
        }
    }
}
