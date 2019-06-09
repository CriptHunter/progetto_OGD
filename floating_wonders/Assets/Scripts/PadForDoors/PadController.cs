using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PadController : NetworkBehaviour
{
    /*[SerializeField] private List<Pad> padsList;
    private bool allPadPressed;
    private bool doorOpen;

    private void Start()
    {
        allPadPressed = false;
        doorOpen = false;
    }

    private void Update()
    {
        int i = 0;
        int cont = 0;
        while (i < padsList.Count)
        {
            if (padsList[i].GetPadPressed())
            {
                cont++;
            }
            i++;
        }

        if (cont == padsList.Count)
        {
            allPadPressed = true;
            OpenDoor();
        }
        else
        {
            allPadPressed = false;
            CloseDoor();
        }
             
    }

    private void OpenDoor()
    {
        if (allPadPressed && !doorOpen)
        {
            transform.position += new Vector3(0, 4, 0);
            doorOpen = true;
        }
    }
    private void CloseDoor()
    {
        if (!allPadPressed && doorOpen)
        {
            transform.position -= new Vector3(0, 4, 0);
            doorOpen = false;
        }
    }*/

    [SerializeField] private List<Pad> padsList;
    [SyncVar(hook = "ChangeStatus")] private bool isOpened = false;

    private void Update()
    {
        if (!isServer)
            return;

        int padCont = 0;
        foreach(Pad pad in padsList)
        {
            if (pad.GetPadPressed())
                padCont++;
        }
        if (padCont == padsList.Count && !isOpened)
            isOpened = true;
        else if (padCont != padsList.Count && isOpened)
            isOpened = false;
    }

    private void ChangeStatus(bool isOpened)
    {
        this.isOpened = isOpened;
        if (isOpened)
            OpenDoor();
        else
            CloseDoor();
    }

    private void OpenDoor()
    {
        transform.position += new Vector3(0, 4, 0);
    }

    private void CloseDoor()
    {
        transform.position -= new Vector3(0, 4, 0);
    }



}
