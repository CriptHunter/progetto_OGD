using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlatformMovToObj : NetworkBehaviour
{
    /*private struct ObjOnPlatform {
        private GameObject target;
        private Vector3 offset;

        public void SetTarget(GameObject target) {
            this.target = target;
        }

        public void SetOffset(Vector3 offset)
        {
            this.offset = offset;
        }

        public GameObject GetTarget()
        {
            return target;
        }

        public Vector3 GetOffset()
        {
            return offset;
        }
    }

    private List<ObjOnPlatform> objList;
    
    void Start()
    {
        //ObjOnPlatform objPlt = new ObjOnPlatform();
        objList = new List<ObjOnPlatform>();
        //objPlt.target = null;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        ObjOnPlatform objPlt = new ObjOnPlatform();
        
        objPlt.SetTarget(col.gameObject);
        objPlt.SetOffset(objPlt.GetTarget().transform.position - transform.position);

        objList.Add(objPlt);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        for(int i = 0; i < objList.Count; i++)
        {
            if(objList[i].GetTarget() == col.gameObject)
                objList.Remove(objList[i]);
        }
    }

    void Update()
    {
        if (objList.Count > 0)
        {
            if (isServer)
            {
                RpcAdjustPosition();
            }
            else
                CmdAdjustPosition();
            
                
        }
    }

    [Command]
    private void CmdAdjustPosition()
    {
        RpcAdjustPosition();
    }

    [ClientRpc]
    private void RpcAdjustPosition()
    {
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].GetTarget().transform.position = transform.position + objList[i].GetOffset();
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision.transform.parent = gameObject.transform.parent.parent;
        
        Cmd_SetParent(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.transform.parent = null;
    }

    [Command] private void Cmd_SetParent(GameObject g)
    {
        Rpc_SetParent(g);
    }

    [ClientRpc] private void Rpc_SetParent(GameObject g)
    {
        g.transform.parent = gameObject.transform.parent.parent;
    }

}
