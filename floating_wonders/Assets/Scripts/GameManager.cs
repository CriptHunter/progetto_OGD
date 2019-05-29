using System;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    [SerializeField] [SyncVar(hook = "SetHealth")] private int health;
    [SerializeField] [SyncVar(hook = "SetKeys")] private int keys;
    [SerializeField] [SyncVar(hook = "SetGems")] private int gems;
    private PlayerHUD hud;
    //singleton
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Start()
    {
        hud = GameObject.Find("HUD").GetComponent<PlayerHUD>();
    }

    public void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        hud.SetHealthText(this.health);
        hud.setKeysText(this.keys);
        hud.setGemsText(this.gems);
    }

    public void TakeDamage(int damage)
    {
        SetHealth(health - damage);
    }

    public void SetHealth(int health)
    {
        this.health = health;
        if (health <= 0)
            print("morti");
    }

    public void SetKeys(int keys)
    {
        if (keys >= 0)
            this.keys = keys;
        else
            keys = 0;
    }

    public void SetGems(int gems)
    {
        if (gems >= 0)
            this.gems = gems;
        else
            gems = 0;
    }

    public void GrabCollectible(ItemType collectible)
    {
        switch (collectible)
        {
            case ItemType.key:
                SetKeys(this.keys + 1);
                break;
            case ItemType.gem:
                SetGems(this.gems + 1);
                break;
        }
    }
}
