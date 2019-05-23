using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Spine.Unity;

public class SpineCharacterAnimator : NetworkBehaviour
{
    private SkeletonAnimation skeletonAnimation;

    public bool DontReloadSameAnimation { get; set; }
    // Start is called before the first frame update
    private void Start()
    {
        var kid = gameObject.GetChild("Spine GameObject");
        if (kid != null)
        {
            skeletonAnimation = kid.GetComponent<SkeletonAnimation>();
        }
    }
    public string Animation
    {
        get
        {
            return SkeletonAnimationGet();
        }
        set
        {
            SkeletonAnimationSet(value,DontReloadSameAnimation);
        }
    }

    public float AnimationSpeed
    {
        get
        {
            return skeletonAnimation.AnimationState.TimeScale;
        }
        set
        {
            skeletonAnimation.AnimationState.TimeScale = value;
        }
    }

    public void SkeletonAnimationSet(string animation, bool dontReloadSameAnimation=false)
    {
        CmdSkeletonAnimationSet(animation, dontReloadSameAnimation);
    }


    [Command]
    private void CmdSkeletonAnimationSet(string animation, bool dontReloadSameAnimation)
    {
        RpcSkeletonAnimationSet(animation, dontReloadSameAnimation);
    }

    [ClientRpc]
    private void RpcSkeletonAnimationSet(string animation, bool dontReloadSameAnimation)
    {
        if (skeletonAnimation != null)
        {
            if (!(animation == Animation && dontReloadSameAnimation))
            {
                skeletonAnimation.AnimationName = animation;
            }
        }
    }

    public string SkeletonAnimationGet()
    {
        if (skeletonAnimation != null)
        {
            return skeletonAnimation.AnimationName;
        }
        else
        {
            return "";
        }
    }
}
