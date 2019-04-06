using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



//un gameobject con questo componente è un oggetto che si può raccogliere e utilizzare
public class Pickuppable : NetworkBehaviour
{
    [SerializeField] private EnumCollection.itemsEnum type;
    public EnumCollection.itemsEnum Type { get { return type; }}
}
