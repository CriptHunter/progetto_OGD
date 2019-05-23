using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private Text keysText;
    [SerializeField] private Text gemsText;

    public void SetHealthText(int health)
    {
        healthText.text = "HEALTH = " + health.ToString();
    }

    public void setKeysText(int keys)
    {
        keysText.text = "KEYS = " + keys.ToString();
    }

    public void setGemsText(int gems)
    {
        gemsText.text = "GEMS = " + gems.ToString();
    }
}
