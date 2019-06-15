using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pad : MonoBehaviour
{
    private bool padPressed;
    private List<GameObject> playerOnPad;
    private LayerMask playerMask;
    private SkeletonAnimation skeletonAnimation;

    public void SetPadPressed(bool padPressed)
    {
        this.padPressed = padPressed;
        if(padPressed)
            skeletonAnimation.AnimationName = "down";
        else
            skeletonAnimation.AnimationName = "up";

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
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //per non far collidere con lo strike collider del giocatore
        if (collision is CircleCollider2D)
            return;

        SetPadPressed(true);
        playerOnPad.Add(collision.gameObject);

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (playerOnPad.Contains(collision.gameObject))
        {
            playerOnPad.Remove(collision.gameObject);
            if (playerOnPad.Count == 0)
                SetPadPressed(false);
        }
    }
}
