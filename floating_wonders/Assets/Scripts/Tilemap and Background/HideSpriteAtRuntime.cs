using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpriteAtRuntime : MonoBehaviour
{
    void Start()
    {
        Destroy(GetComponent<SpriteRenderer>());
    }
}
