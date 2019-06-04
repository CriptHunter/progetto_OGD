using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    [SerializeField]  private int health;
    [SerializeField]  private int keys;
    [SerializeField] private int gems;

    [SerializeField] private int maxHealth;
    [SerializeField] private int maxKeys;
    [SerializeField] private int maxGems;

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
        hud.health = this.health + "/" + this.maxHealth;
        hud.keys = this.keys + "/" + this.maxKeys;
        hud.gems = this.gems + "/" + this.maxGems;
    }

    public void TakeDamage(GameObject damagedPlayer,int damage)
    {
        StartCoroutine(Stun(damagedPlayer));
        SetHealth(health - damage);
    }

    public void SetHealth(int health)
    {
        this.health = health;
        if (health <= 0)
        {
            SetHealth(maxHealth);
            CheckPointsManager.Instance.Respawn();
        }
    }

    public void SetKeys(int keys)
    {
        if (keys >= 0)
            this.keys = keys;
        else
            keys = 0;
    }

    public int GetKeys()
    {
        return keys;
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

    private IEnumerator Stun(GameObject player)
    {
        Rpc_SetSpineColor(player, Color.red);
        player.GetComponent<AnotherCharacterInput>().enabled = false;
        yield return new WaitForSeconds(.3f);
        Rpc_SetSpineColor(player, Color.white);
        player.GetComponent<AnotherCharacterInput>().enabled = true;
    }

    [ClientRpc] private void Rpc_SetSpineColor(GameObject player, Color color)
    {
        player.GetComponentInChildren<SkeletonAnimation>().skeleton.SetColor(color);
    }


}
