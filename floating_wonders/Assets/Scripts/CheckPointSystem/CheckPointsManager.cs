using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CheckPointsManager : NetworkBehaviour
{
    private List<CheckPoint> checkPointList;
    private PlayerList pList;
    private EnemyList eList;
    private CheckPoint activeCheck;


    // Start is called before the first frame update
    void Awake()
    { 
        checkPointList = new List<CheckPoint>();
    }

    // Update is called once per frame
    void Update()
    {
       /* for (int i = 0; i < checkPointList.Count; i++)
        {
            if (checkPointList[i].GetTriggered())
            {
                activeCheck = checkPointList[i];
            }
            else 
                activeCheck = null;
            checkPointList[i].SetStatus(false);
        }*/

       /* if (activeCheck != null)
            print("ActiveCheckPoint: " + GetCheckPoint());*/

    }

    public CheckPoint GetCheckPoint()
    {
        return activeCheck;
    }

    public void AddCheckPoint(CheckPoint c)
    {
        checkPointList.Add(c);
    }

    public void SetActiveCheckPoint(CheckPoint c)
    {
        this.activeCheck = c;
    }

    public void GetPlayerEnemyList()
    {
        pList = GameObject.Find("NetworkManager").GetComponent<PlayerList>();
        eList = GameObject.Find("NetworkManager").GetComponent<EnemyList>();
    }

    public void Respawn()
    {
        GetPlayerEnemyList();
        foreach (Enemy e in eList.GetEnemyList())
        {
            e.gameObject.SetActive(false);
        }
        foreach (GameObject p in pList.GetPlayerList())
        {
            p.SetActive(false);
            p.transform.position = this.GetCheckPoint().transform.position;
            p.SetActive(true);
        }
        foreach (Enemy e in eList.GetEnemyList())
        {
            e.gameObject.transform.position = e.GetStartingPosition();
            e.gameObject.SetActive(true);
        }

    }
}
