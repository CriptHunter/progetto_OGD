using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadController : MonoBehaviour
{
    [SerializeField]
    private List<Pad> padsList;
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
            Debug.Log(padsList[i].GetPadPressed());
            if (padsList[i].GetPadPressed())
            {
                Debug.Log("Pressed: " + padsList[i].name);
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
    }
}
