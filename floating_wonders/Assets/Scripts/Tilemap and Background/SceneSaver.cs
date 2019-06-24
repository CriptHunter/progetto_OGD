using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneSaver : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl)){
            if (Input.GetKeyDown(KeyCode.S)){
                Save();
            }
        }
    }

    public void Save()
    {
        print("niente");
    }
}
