using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CheckPointsManager : NetworkBehaviour
{
    private List<CheckPoint> checkPointList;
    private List<Enemy> enemyList;
    private List<GameObject> playerList;
    private CheckPoint activeCheck;
    private static CheckPointsManager instance;
    public static CheckPointsManager Instance { get { return instance; } }


    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        checkPointList = new List<CheckPoint>();
        enemyList = new List<Enemy>();
        playerList = new List<GameObject>();   
    }

    void Update()
    {
        
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

    public void AddEnemy(Enemy e)
    {
        enemyList.Add(e);
        print("dimensione lista " + enemyList.Count);
    }

    public List<Enemy> GetEnemyList()
    {
        return enemyList;
    }

    /*public void GetPlayerEnemyList()
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

    }*/
}
