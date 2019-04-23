using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumCollection : MonoBehaviour
{
    //lista di oggetti che si possono raccogliere e lanciare
    public enum ItemType : int
    {
        nullItem = -1,
        bomb = 0,
        grapplingHook = 1,
    };
}
