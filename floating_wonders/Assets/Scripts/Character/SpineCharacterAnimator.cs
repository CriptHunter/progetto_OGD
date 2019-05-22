using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineCharacterAnimator : MonoBehaviour
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

    public void SkeletonAnimationSet(string animation, bool dontReloadSameAnimation=false)
    {
        if (skeletonAnimation != null)
        {
            if (!(animation==Animation && dontReloadSameAnimation))
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
