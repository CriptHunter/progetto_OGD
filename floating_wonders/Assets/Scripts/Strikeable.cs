using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This clas is a component that marks an object as being able to get hit by a StrikeController
/// </summary>
public abstract class Strikeable : MonoBehaviour
{
    /// <summary>
    /// This abstract method will be called if the collider of this gameobject is hit by the StrikeController
    /// </summary>
    public abstract void Strike(GameObject attacker, int damage);
}
