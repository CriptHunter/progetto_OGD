using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Metodi comuni ai diversi tipi di movimento dei nemici
/// </summary>
public abstract class EnemyBehaviour : NetworkBehaviour
{
    [SyncVar (hook = "ChangeDirection")] protected bool movingRight;
    public abstract void ChangeDirection(bool movingRight);
}
