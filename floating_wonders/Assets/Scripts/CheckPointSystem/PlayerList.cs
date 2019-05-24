using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    private List<GameObject> pList;

    // Start is called before the first frame update
    void Start()
    {
        pList = new List<GameObject>();
    }

    public void AddPlayer(GameObject p)
    {
        pList.Add(p);
    }

    public List<GameObject> GetPlayerList()
    {
        return pList;
    }
}
