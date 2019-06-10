﻿using System.Collections;
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

    public CheckPoint GetActiveCheckPoint()
    {
        return activeCheckPoint;
    }

    public void SetActiveCheckPoint(CheckPoint c)
    {
        if (!isServer)
            return;
        this.activeCheckPoint = c;
        Rpc_ShowColor(Color.white, GetActiveCheckPoint().gameObject);
        ResetEnemies();
        ResetPlayers();
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
        if (GetActiveCheckPoint() == null)
            print("Non ci sono checkpoint attivi");
        else
        {
            Rpc_ShowColor(Color.white, GetActiveCheckPoint().gameObject);
            ResetEnemies();
            ResetPlayers();
        }
    }

    public void ResetEnemies()
    {
        foreach (Enemy e in enemyList)
        {
            Rpc_SetEnemyRespawnPosition(e.gameObject);
            e.GetComponent<EnemyHealth>().SetFullHealth();
        }
    }

    public void ResetPlayers()
    {
        foreach (GameObject p in playerList)
        {
            Rpc_SetPlayerRespawnPosition(p, activeCheckPoint.gameObject);
            GameManager.Instance.SetHealth(GameManager.Instance.GetMaxHealth());
        }
    }

    [ClientRpc] private void Rpc_ShowColor(Color c, GameObject ck)
    {
        StartCoroutine(ShowColor(c, ck));
    }

    [ClientRpc] private void Rpc_SetEnemyRespawnPosition(GameObject enemy)
    {
        enemy.SetActive(false);
        enemy.transform.position = enemy.GetComponent<Enemy>().GetStartingPosition();
        enemy.SetActive(true);
    }

    [ClientRpc] private void Rpc_SetPlayerRespawnPosition(GameObject player, GameObject checkpoint)
    {
        AnotherCharacterController controller = player.GetComponent<AnotherCharacterController>();
        //stacco il personaggio dai bordi altrimenti non si teletrasporta
        if(controller.IsDanglingFromEdge())
            player.GetComponent<AnotherCharacterController>().ReleaseEdge();
        if(controller.IsClimbing())
            player.GetComponent<AnotherCharacterController>().ReleaseClimbable();
        player.SetActive(false);
        player.transform.position = checkpoint.transform.position;
        player.SetActive(true);
    }

    private IEnumerator ShowColor(Color c, GameObject ck)
    {
        ck.SetActive(false);
        hud.ShowColor(true, c);
        yield return new WaitForSeconds(1f);
        hud.ShowColor(false, c);
        ck.SetActive(true);
    }
}