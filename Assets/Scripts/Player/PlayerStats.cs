using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //hidden stats (hidden to the player)
    public int maxHealth;
    public int currentHealth;

    //leaderboard stats
    public int points;
    public int kills;
    public int downs;
    public int revives;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //getters and setters
    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public void SetCurrentHealth(int newHealth)
    {
        currentHealth = newHealth;
    }
    public void AddHealth(int health)
    {
        currentHealth += health;
    }
    public void Add10Points()
    {
        points += 10;
    }
    public void Add100Points()
    {
        points += 100;
    }
    public void AddPoints(int points)
    {
        this.points += points;
    }
    public void IncrementKillCounter()
    {
        kills++;
    }
    public void IncrementDownCounter()
    {
        downs++;
    }
    public void IncrementRevivesCounter() 
    {  
        revives++;
    }
}
