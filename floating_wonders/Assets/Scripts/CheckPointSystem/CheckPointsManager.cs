using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointsManager : MonoBehaviour
{
    private List<CheckPoint> checkPointList;

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

    public void Respawn(GameObject g)
    {
        g.transform.position = this.GetCheckPoint().transform.position;
        g.SetActive(true);
    }
}
