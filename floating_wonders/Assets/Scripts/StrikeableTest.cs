using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeableTest : Strikeable
{
    public override void Strike(GameObject attacker)
    {
        Destroy(gameObject);
    }
}
