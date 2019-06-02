using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CheckPointsManager : NetworkBehaviour
{
    private List<CheckPoint> checkPointList;
    private List<Enemy> enemyList;
    private List<GameObject> playerList;
    private CheckPoint activeCheckPoint;
    private PlayerHUD hud;
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

    public void Start()
    {
        hud = GameObject.Find("HUD").GetComponent<PlayerHUD>();
    }

    public CheckPoint GetCheckPoint()
    {
        return activeCheckPoint;
    }

    public void SetActiveCheckPoint(CheckPoint c)
    {
        this.activeCheckPoint = c;
    }

    public void AddCheckPoint(CheckPoint c)
    {
        checkPointList.Add(c);
    }

    public void AddEnemy(Enemy e)
    {
        enemyList.Add(e);
    }

    public void AddPlayer(GameObject g)
    {
        playerList.Add(g);
    }

    public void Respawn()
    {
        if (!isServer)
            return;
        if (GetCheckPoint() == null)
            print("Non ci sono checkpoint attivi");
        else
        {
            Rpc_ShowBlackScreen();
            foreach (GameObject p in playerList)
                Rpc_SetPlayerRespawnPosition(p, activeCheckPoint.gameObject);
            foreach (Enemy e in enemyList)
                Rpc_SetEnemyRespawnPosition(e.gameObject);
        }
    }

    [ClientRpc] private void Rpc_ShowBlackScreen()
    {
        StartCoroutine("blackScreen");
    }

    [ClientRpc] private void Rpc_SetEnemyRespawnPosition(GameObject enemy)
    {
        enemy.transform.position = enemy.GetComponent<Enemy>().GetStartingPosition();
        enemy.SetActive(true);
    }

    [ClientRpc] private void Rpc_SetPlayerRespawnPosition(GameObject player, GameObject checkpoint)
    {
        print("sono " + player + " e mi sposto al checkpoint in posizione " + checkpoint.transform.position);
        player.transform.position = checkpoint.transform.position;
    }

    private IEnumerator blackScreen()
    {
        hud.showBlackScreen(true);
        yield return new WaitForSeconds(2f);
        hud.showBlackScreen(false);
    }
}
