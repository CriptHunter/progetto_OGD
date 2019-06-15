using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CheckPoint : NetworkBehaviour
{
    private LayerMask playerMask;
    private int status;
    private int lastStatus;
    private SkeletonAnimation skeletonAnimation;

    void Start()
    {
        status = 0;
        playerMask = 1 << 9;
        CheckPointsManager.Instance.AddCheckPoint(this);
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }

    private void Update()
    {
        //coloro i checkpoint in base al loro stato
        if(status == 0)
            skeletonAnimation.AnimationName = "inactive";
        if (status == 2)
            skeletonAnimation.AnimationName = "semiactive";
        if (status == 4)
        {
            skeletonAnimation.AnimationName = "active";
            if (isServer && lastStatus != 4)
                CheckPointsManager.Instance.SetActiveCheckPoint(this);
        }
        lastStatus = status;
        //faccio un raycast circolare intorno al checkpoint
        //RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, 1, Vector2.zero, 0, playerMask);
        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position, new Vector2(3f, 3f), 0, Vector2.zero, 0, playerMask);
        status = 0;
        //conto il numero di giocatori intorno al checkpoint, faccio così perché la lunghezza dell'array hit comprende anche i child object
        foreach(RaycastHit2D h in hit)
        {
            if (h.collider.GetComponent<AnotherCharacterController>() != null)
                status++;
        }
    }
}
