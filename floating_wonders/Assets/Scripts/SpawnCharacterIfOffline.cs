using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacterIfOffline : MonoBehaviour
{
    public GameObject character;
    private float countdown = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
        }
        else
        {
            var go = Instantiate(character);
            go.transform.position = transform.position;
            Destroy(go.GetComponent<SetupLocalPlayer>());
            go.GetComponent<AnotherCharacterController>().enabled = true;
            go.GetComponent<AnotherCharacterInput>().enabled = true;
            Camera.main.gameObject.GetComponent<CameraController>().target = go;
            Destroy(this);
        }
    }
}
