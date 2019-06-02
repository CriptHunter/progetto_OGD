using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spikes : NetworkBehaviour
{
    //se le spikes sono sempre visibili o possono nascondersi sotto trra
    [SerializeField] private bool canHide;
    //tempo in cui le spikes sono nascoste/visibili
    [SerializeField] private float hiddenTime;
    private float timer;
    [SyncVar(hook = "ChangeVisibility")] private bool isVisible;
    //altezza delle spikes, utile perchè in questo modo spikes di qualsiasi altezza si nascondono completamente nel terreno
    private float spikeHeight;

    void Start()
    {
        isVisible = false;
        timer = 0;
        spikeHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {   if (canHide)
        {
            timer = timer + Time.deltaTime;
            if (timer > hiddenTime)
            {
                timer = 0;
                if (isServer)
                    isVisible = !isVisible;
            }
        }
    }

    private void ChangeVisibility(bool isVisible)
    {
        if (!isVisible)
            transform.Translate(Vector2.up * spikeHeight);
        else
            transform.Translate(Vector2.down * spikeHeight);
        this.isVisible = !isVisible;
    }
}
