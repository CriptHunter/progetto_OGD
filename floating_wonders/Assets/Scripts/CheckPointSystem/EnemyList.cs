using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    private List<Enemy> eList;
    
    // Start is called before the first frame update
    void Start()
    {
        eList = new List<Enemy>();
    }

    public List<Enemy> GetEnemyList ()
    {
        return eList;
    }

    public void AddEnemy(Enemy e)
    {
        eList.Add(e);
    }

}
