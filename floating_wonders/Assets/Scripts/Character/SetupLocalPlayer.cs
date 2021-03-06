﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using Spine;
using Spine.Unity;


public class SetupLocalPlayer : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //nasconde la freccia del mouse
        //Cursor.visible = false;

        //per il respawn ai checkpoint
        try
        {
            var cpm = CheckPointsManager.Instance;
            if (cpm != null)
            {
                CheckPointsManager.Instance.AddPlayer(this.gameObject);
            }

        } catch(NullReferenceException e)
        {
            print("there is no checkpoint manager "+e);
        }

        //Il server ha il braccio steampunk, il client il rampino
        var gh = GetComponent<GrapplingHook>();
        var ea = this.GetComponent<ExtendableArm>();
        var itemManager = this.GetComponent<ItemManager>();
        var skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();

        if (gh != null && ea != null)
        {
            if (isServer && isLocalPlayer)
            {
                gh.enabled = false;
                ea.enabled = true;
                itemManager.SetUniqueItem(ItemType.extendableArm);
            }
            else if (!isServer && isLocalPlayer)
            {
                gh.enabled = true;
                ea.enabled = false;
                itemManager.SetUniqueItem(ItemType.grapplingHook);
            }



            
            if (isServer )
            {
                if (isLocalPlayer)
                {
                    skeletonAnimation.skeleton.SetSkin("sk_male");
                    // setto l'icona nella minimappa
                    PlayerMinimapIndicator pmi = GameObject.Find("HUD").GetChild("Minimap").GetChild("player1").GetComponent<PlayerMinimapIndicator>();
                    pmi.SetTarget(gameObject);
                }
                else
                {
                    skeletonAnimation.skeleton.SetSkin("sk_female");
                    // setto l'icona nella minimappa
                    PlayerMinimapIndicator pmi = GameObject.Find("HUD").GetChild("Minimap").GetChild("player2").GetComponent<PlayerMinimapIndicator>();
                    pmi.SetTarget(gameObject);
                }
            }
            else
            {
                if (!isLocalPlayer)
                {
                    skeletonAnimation.skeleton.SetSkin("sk_male");
                    // setto l'icona nella minimappa
                    PlayerMinimapIndicator pmi = GameObject.Find("HUD").GetChild("Minimap").GetChild("player1").GetComponent<PlayerMinimapIndicator>();
                    pmi.SetTarget(gameObject);
                }
                else
                {
                    skeletonAnimation.skeleton.SetSkin("sk_female");
                    // setto l'icona nella minimappa
                    PlayerMinimapIndicator pmi = GameObject.Find("HUD").GetChild("Minimap").GetChild("player2").GetComponent<PlayerMinimapIndicator>();
                    pmi.SetTarget(gameObject);
                }
            }
        }

        if (!isLocalPlayer)
        {
            AnotherCharacterInput anotherCaracterInput = GetComponent<AnotherCharacterInput>();
            if (anotherCaracterInput != null)
            {
                anotherCaracterInput.enabled = false;
            }
            AnotherCharacterController anotherCaracterController = GetComponent<AnotherCharacterController>();
            if (anotherCaracterController != null)
            {
                anotherCaracterController.enabled = false;
            }
        }
        else
        {
            CameraController controller = Camera.main.GetComponent<CameraController>();
            if (controller != null)
            {
                controller.target = gameObject;
            }
        }
    }
}
