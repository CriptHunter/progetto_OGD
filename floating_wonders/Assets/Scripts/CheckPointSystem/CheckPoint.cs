using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private LayerMask playerMask;
    private int status;

    void Start()
    {
        status = 0;
        playerMask = LayerMask.NameToLayer("Player");
        CheckPointsManager.Instance.AddCheckPoint(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.layer == playerMask)
        {
            status++;
            if (status == 2)
                CheckPointsManager.Instance.SetActiveCheckPoint(this);
        }
        print("ontriggerenter " + status);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        status--;
        print("onTriggerexit " + status);
    }
}
