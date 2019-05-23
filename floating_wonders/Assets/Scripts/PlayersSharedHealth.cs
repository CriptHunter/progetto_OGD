using UnityEngine;

public class PlayersSharedHealth : MonoBehaviour
{
    [SerializeField] private int health;

    public void Start()
    {
    }
    public void TakeDamage(int damage)
    { 
        health = health - damage;
        print("vita personaggio " + health);
    }

    //viene chiamato ogni volta che la vita cambia
    void OnChangeHealth(int newHealth)
    {
        health = newHealth;
        print("vita personaggi: " + health);
        if (health <= 0)
            this.gameObject.SetActive(false);
    }
}
