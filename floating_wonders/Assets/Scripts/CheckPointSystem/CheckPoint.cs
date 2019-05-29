using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private LayerMask playerMask;
    private bool triggered;
    [SerializeField] private GameObject levelManager;
    private CheckPointsManager c;
    private GameObject firstPlayerOnCheckPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerMask = LayerMask.NameToLayer("Player");
        triggered = false;
        //gm = GameObject.Find("LevelManager");
        CheckPointsManager.Instance.AddCheckPoint(this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.layer == playerMask)
        {
            Debug.Log("Collisione CheckPoint " + this.name + " - player");
            if (firstPlayerOnCheckPoint == null)
                firstPlayerOnCheckPoint = collision.gameObject;
            else
            {
                if (firstPlayerOnCheckPoint != collision.gameObject)
                {
                    triggered = true;
                    CheckPointsManager.Instance.SetActiveCheckPoint(this);
                }
            }

            /*Debug.Log("Collisione CheckPoint " + this.name + " - player");
            triggered = true;
            c.SetActiveCheckPoint(this);*/
        }
    }

    public bool GetTriggered()
    {
        return triggered;
    }

    public void SetStatus(bool status)
    {
        triggered = status;
    }
}
