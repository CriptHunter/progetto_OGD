using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadKeyUnlockDoor : MonoBehaviour
{
    private LayerMask player;


    // Start is called before the first frame update
    void Start()
    {
        player = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == player)
        {
            this.GetComponentInParent<KeyUnlockDoor>().CheckDoor();
        }
    }
}
