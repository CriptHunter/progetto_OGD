using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

using Spine;
using Spine.Unity;

public class SpineCharacterAnimator : NetworkBehaviour
{
    public UnityEvent OnAttack;
    public UnityEvent OnFootstep;

    private SkeletonAnimation skeletonAnimation;

    private float resetTimer = 0;
    private bool resetPerformed = true;

    public bool DontReloadSameAnimation { get; set; }
    // Start is called before the first frame update
    private void Start()
    {
        var kid = gameObject.GetChild("Spine GameObject");
        if (kid != null)
        {
            skeletonAnimation = kid.GetComponent<SkeletonAnimation>();
            skeletonAnimation.AnimationState.Event += HandleEvent;
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
            SkeletonAnimationSet(value, DontReloadSameAnimation);
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

    public void SkeletonAnimationOverlay(string animation, bool enqueue = false)
    {
        CmdSkeletonAnimationOverlay(animation, enqueue);
    }

    public void SkeletonAnimationSet(string animation, bool dontReloadSameAnimation = false)
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

    [Command]
    private void CmdSkeletonAnimationOverlay(string animation, bool enqueue)
    {
        RpcSkeletonAnimationOverlay(animation, enqueue);
    }

    [ClientRpc]
    private void RpcSkeletonAnimationOverlay(string animation, bool enqueue)
    {
        if (skeletonAnimation != null)
        {
            if (!enqueue)
            {
                skeletonAnimation.state.SetAnimation(1, animation, false);
                resetTimer = 0.5f;
                resetPerformed = false;
            }
            else
            {
                if (resetPerformed)
                {
                    skeletonAnimation.state.SetAnimation(1, animation, false);
                    resetTimer = 0.5f;
                    resetPerformed = false;
                }
            }

            //skeletonAnimation.state.AddAnimation(1, "", false, 0);
            //skeletonAnimation.state.AddEmptyAnimation(1,0.2f,0);
            //skeletonAnimation.state.SetEmptyAnimation(1, 0.2f);
            //skeletonAnimation.state.AddAnimation(1, animation, false, 0);
            //skeletonAnimation.state.AddEmptyAnimation(1, 0.2f, 0);
        }
    }

    private void Update()
    {
        if (resetTimer > 0)
        {
            resetTimer -= Time.deltaTime;
        }
        else
        {
            resetTimer = 0;
            if (!resetPerformed)
            {
                resetPerformed = true;
                if (skeletonAnimation != null)
                {
                    skeletonAnimation.state.SetEmptyAnimation(1, 0.05f);
                }
            }
        }
    }

    public bool isTrack1Playing()
    {
        return !resetPerformed;
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

    private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "ev_strike")
        {
            if (OnAttack != null)
            {
                OnAttack.Invoke();
            }
        }

        if (e.Data.Name == "ev_footstep")
        {
            if (OnFootstep != null)
            {
                OnFootstep.Invoke();
            }
        }
    }
}
