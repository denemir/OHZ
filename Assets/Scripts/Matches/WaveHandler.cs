using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    // Start is called before the first frame update

    //wave details
    public int currentWave;
    private bool isWaveActive;

    //zombies
    public ZombiePool pool;
    //public List<int> zombiesRemaining; //zombies remaining in every wave
    //public List<int> zombiesKilled; //zombies killed in every wave

    public int zombieCount;
    public int numberOfZombiesForRound; //max number of zombies for this round
    private bool areZombiesEliminated; //if zombiePool is full, all zombies are eliminated

    //dogs
    private bool isDogRound;
    public List<int> hellHoundsRemaining;
    public List<int> hellHoundsKilled;

    //powerups
    public List<Powerup> powerups;
    private int powerupsDroppedThisRound = 0; //max of 4

    //spawn
    public float spawnDelay;
    public float delayTimer;

    //number of players
    private int numberOfPlayersInMatch; //should be passed down from match handler

    //vars

    void Start()
    {
        currentWave = 0;
        BeginWave();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int temp = 0;
        //if wave is currently going
        if (isWaveActive)
        {
            if (delayTimer > 0)
                delayTimer -= 0.005f;

            if (zombieCount < numberOfZombiesForRound && delayTimer <= 0 && pool.GetNumberOfAvailableZombies() > 0) //prepare to spawn if timer is ready and zombies still have remaining units
            {
                SpawnZombie();
                temp++;
            }
            else if (zombieCount >= numberOfZombiesForRound && areZombiesEliminated)
            {
                EndWave();
                BeginWave();
            }

            if (pool.IsPoolFull() && zombieCount == numberOfZombiesForRound)
            {
                areZombiesEliminated = true;
            }                
            else areZombiesEliminated = false;
        }
    }

    //spawning
    /// <summary>
    /// CREDIT TO PROJECT LAZARUS: ZOMBIES FOR FORMULAS RELATED TO SPAWNING
    /// </summary>
    /// <param name="round"></param>
    /// <returns></returns>
    private int CalculateNumberOfZombiesBelowRound20(int round)
    {
        float zombieCount = 0f;
        switch (numberOfPlayersInMatch)
        {
            case 1:
                zombieCount = 10.6142f * Mathf.Pow(1.101f, round);
                Debug.Log(zombieCount);
                break;
            case 2:
                zombieCount = 0.1793f * Mathf.Pow(round, 2) + 0.0405f * round + 23.187f;
                break;
            case 3:
                zombieCount = 0.262f * Mathf.Pow(round, 2) + 0.301f * round + 33.114f;
                break;
            default:
                zombieCount = 6.1142f * Mathf.Pow(1.135f, round);
                break;
        }

        return (int)zombieCount;
    }
    private int CalculateNumberOfZombiesAboveRound20(int round)
    {
        float zombieCount = 0f;
        switch (numberOfPlayersInMatch)
        {
            //case 0:
            //    break;
            case 1:
                zombieCount = Mathf.Min((float)(0.09 * Mathf.Pow(round, 2) - 0.0029 * round + 23.9580), 100);
                break;
            case 2:
                zombieCount = Mathf.Min((float)(0.18820 * Mathf.Pow(round, 2) - 0.4313 * round + 29.2120), 175);
                break;
            default: //default accounts for 3-4 players or above
                zombieCount = /*Mathf.Min(*/(float)(0.26370 * Mathf.Pow(round, 2) - 0.1802 * round + 35.015)/*, 265)*/; //teehee!
                break;
        }
        return (int)zombieCount;
    }
    private float CalculateSpawnDelay(int round)
    {
        return Mathf.Max(2f * (Mathf.Pow(0.95f, round - 1)), 0.1f);
    }
    private int CalculateZombieHealth(int round)
    {
        int zombieHealth = 150; //150 from round 1
        switch(round < 10)
        {
            case true: //if round is less than 10, zombieHealth is this formula, equivalating to about 1050 in the end
                zombieHealth = 50 + 100 * round;
                break;
            case false: //otherwise its time to go gremlin mode
                zombieHealth = (int)(950 * Mathf.Pow(1.1f, round - 9));
                break;
        }
        return zombieHealth;
    }
    private float CalculateZombieMoveSpeed(int round)
    {
        float moveSpeed = 0f;
        switch(round < 10)
        {
            case true:
                moveSpeed = 6f + 0.5f * round;
                break;
                case false:
                moveSpeed = 11f;
                break;
        }
        return moveSpeed;
    }

    //waves
    private void BeginWave()
    {
        //increment wave counter
        currentWave++;
        isWaveActive = true;

        //prepare for wave
        switch (currentWave >= 20)
        {
            case true:
                numberOfZombiesForRound = CalculateNumberOfZombiesAboveRound20(currentWave);
                break;
            case false:
                numberOfZombiesForRound = CalculateNumberOfZombiesBelowRound20(currentWave);
                break;
        }

        //calculate spawn delay
        spawnDelay = CalculateSpawnDelay(currentWave);

        //update player gui
        UpdatePlayerGUI();
    }
    private void SpawnZombie()
    {       
        bool didSpawn = pool.SpawnZombie();
        if (didSpawn)
        {
            zombieCount++;
            delayTimer = spawnDelay;
        }
    }
    private void EndWave()
    {
        //resetting
        zombieCount = 0;
        powerupsDroppedThisRound = 0;

        //dog rounds
        if (currentWave % 5 == 0)
        {
            isDogRound = true;
        }
        else isDogRound = false;

        isWaveActive = false;

        //scale zombies
        pool.ScaleZombies(CalculateZombieHealth(currentWave), CalculateZombieMoveSpeed(currentWave));
    }

    //hellhound rounds
    private bool IsDogRound()
    {
        if (currentWave == 5)
            return true;
        return false;
    }
    private bool AreDogsEliminated()
    {
        return false;
    }

    //rewarding
    public void IncrementPowerupCounter()
    {

    }
    public void DeterminePowerupDropForZombie(Zombie zombie)
    {
        if(powerupsDroppedThisRound < 4)
        {

            float odds = Random.Range(0, 100);
            if(odds <= 2)
            {
                zombie.DropPowerup(GetRandomPowerup());
            }

        }
    }
    public void DropRewardForFinalHellHound(HellHounds hellhound)
    { } //max ammo

    //powerups
    private Powerup GetRandomPowerup()
    {
        int num = Random.Range(0, powerups.Count);
        return powerups[num];
    }

    //updating gui
    private void UpdatePlayerGUI()
    {
        if(GetComponentInParent<MatchHandler>() != null)
        {
            GetComponentInParent<MatchHandler>().UpdatePlayerGUIWaveCounter(currentWave);
        }
        else Debug.Log("no");
    }
}
