using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHUD : NetworkBehaviour
{
    [SerializeField] public Text healthText;
    [SerializeField] public Text keysText;
    [SerializeField] public Text gemsText;

    [SyncVar(hook = "SetHealthText")] public string health;
    [SyncVar(hook = "SetKeysText")] public string keys;
    [SyncVar(hook = "SetGemsText")] public string gems;


    public void Start()
    {
        SetHealthText(this.health);
        SetKeysText(this.keys);
        SetGemsText(this.gems);
    }

    public void SetHealthText(string health)
    {
        healthText.text = health;
    }

    public void SetKeysText(string keys)
    {
        keysText.text = keys;
    }

    public void SetGemsText(string gems)
    {
        gemsText.text = gems;
    }

    public void showBlackScreen(bool visible)
    {
        Image image = GetComponent<Image>();
        var tempColor = image.color;
        if (visible)
            tempColor.a = 1f;
        else
            tempColor.a = 0f;
        image.color = tempColor;
    }
}
