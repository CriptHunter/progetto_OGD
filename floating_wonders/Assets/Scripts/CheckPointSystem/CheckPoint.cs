using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CheckPoint : NetworkBehaviour
{
    private LayerMask playerMask;
    private int status;

    void Start()
    {
        status = 0;
        playerMask = 1 << 9;
        CheckPointsManager.Instance.AddCheckPoint(this); 
    }

    private void Update()
    {
        //coloro i checkpoint in base al loro stato
        if(status == 0)
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        if (status == 1)
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        if (status == 2)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.magenta;
            if(isServer)
                CheckPointsManager.Instance.SetActiveCheckPoint(this);
        }
        //faccio un raycast circolare intorno al checkpoint
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, 1, Vector2.zero, 0, playerMask);
        status = 0;
        //conto il numero di giocatori intorno al checkpoint, faccio così perché la lunghezza dell'array hit comprende anche i child object
        foreach(RaycastHit2D h in hit)
        {
            if (h.collider.GetComponent<AnotherCharacterController>() != null)
                status++;
        }
    }
}
