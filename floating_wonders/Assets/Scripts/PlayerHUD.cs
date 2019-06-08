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

    public void ShowColor(bool visible, Color color)
    {
        Image image = GetComponent<Image>();
        image.color = new Color(color.r, color.g, color.b, visible ? 1f : 0f);
    }
}
