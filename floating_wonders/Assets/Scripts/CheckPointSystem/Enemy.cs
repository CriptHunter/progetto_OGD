﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyList eList;
    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        CheckPointsManager.Instance.AddEnemy(this);
        startingPos = gameObject.transform.position;
    }

    public Vector3 GetStartingPosition()
    {
        return startingPos;
    }

}
