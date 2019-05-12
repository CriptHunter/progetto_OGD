using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    [SerializeField]private GameObject lm;
    private CheckPointsManager cm;
    private bool died;

    // Start is called before the first frame update
    void Start()
    {
        died = false; ;
        cm = lm.GetComponent<CheckPointsManager>();
    }

    // Update is called once per frame
    void Update()
    {
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
