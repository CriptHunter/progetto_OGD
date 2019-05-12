using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDie : NetworkBehaviour
{
    [SerializeField] private GameObject levelManager;
    private CheckPointsManager cm;
    private bool died;

    // Start is called before the first frame update
    void Start()
    {
        died = false; ;
        cm = levelManager.GetComponent<CheckPointsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;
        if (Input.GetKeyDown("space"))
        {
            died = true;
            this.gameObject.SetActive(false);
            cm.Respawn(this.gameObject);
        }            
    }

    public bool GetStatus()
    {
        return died;
    }

}
