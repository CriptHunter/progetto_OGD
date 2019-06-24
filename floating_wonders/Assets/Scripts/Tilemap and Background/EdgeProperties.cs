using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeProperties : MonoBehaviour
{
    [SerializeField]
    private Verse verse;
    public Verse EdgeVerse
    {
        get { return verse; }
        set { verse = value; }
    }
}
