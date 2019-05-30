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
        healthText.text = health.ToString();
    }

    public void setKeysText(int keys)
    {
        keysText.text = keys.ToString();
    }

    public void setGemsText(int gems)
    {
        gemsText.text = gems.ToString();
    }

    public void showBlackScreen()
    {
        Image image = GetComponent<Image>();
        var tempColor = image.color;
        tempColor.a = 1f;
        image.color = tempColor;
    }
}
