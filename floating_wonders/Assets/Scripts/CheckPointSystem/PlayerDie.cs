using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDie : NetworkBehaviour
{
    [SerializeField] private GameObject levelManager;
    private CheckPointsManager cm;
    private bool died;
    private PlayerList pList;

    // Start is called before the first frame update
    void Start()
    {
        died = false;
        cm = levelManager.GetComponent<CheckPointsManager>();
        pList = GameObject.Find("NetworkManager").GetComponent<PlayerList>();
        pList.AddPlayer(this.gameObject);
    }
    
    // Update is called once per frame
    void Update()
    {
       if (!isLocalPlayer)
            return;
        
        if (Input.GetKeyDown("space"))
        {
            if (cm.GetCheckPoint() == null)
                return;
            print(cm.GetCheckPoint());
            died = true;

            if (isServer)
                Rpc_Respawn();
            else
                Cmd_Respawn();
        }            
    }

    public bool GetStatus()
    {
        return died;
    }

    [Command]
    public void Cmd_Respawn()
    {
        Rpc_Respawn();
    }

    [ClientRpc]
    public void Rpc_Respawn()
    {
        cm.Respawn();
    }
}
