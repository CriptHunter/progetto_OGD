using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private LayerMask playerMask;
    private bool triggered;
    [SerializeField] private GameObject gm;
    private CheckPointsManager c;


    // Start is called before the first frame update
    void Start()
    {
        playerMask = LayerMask.NameToLayer("Player");
        triggered = false;
        //gm = GameObject.Find("LevelManager");
        c = gm.GetComponent<CheckPointsManager>();
        c.AddCheckPoint(this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.layer == playerMask)
        {
            //Debug.Log("Collisione CheckPoint " + this.name + " - player");
            triggered = true;
            c.SetActiveCheckPoint(this);
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
