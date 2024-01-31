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

    //spawn
    public float spawnDelay;
    public float delayTimer;

    //number of players
    private int numberOfPlayersInMatch; //should be passed down from match handler

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
        //float zombieCount;
        //switch (numberOfPlayersInMatch)
        //{

        //}

        return 30;
    }
    private int CalculateNumberOfZombiesAboveRound20(int round)
    {
        float zombieCount;
        switch (numberOfPlayersInMatch)
        {
            case 0:
                break;
            case 1:
                zombieCount = Mathf.Min((float)(0.09 * Mathf.Pow(round, 2) - 0.0029 * round + 23.9580), 100);
                break;
            case 2:
                zombieCount = Mathf.Min((float)(0.18820 * Mathf.Pow(round, 2) - 0.4313 * round + 29.2120), 175);
                break;
            default: //default accounts for 3-4 players or above
                zombieCount = Mathf.Min((float)(0.26370 * Mathf.Pow(round, 2) - 0.1802 * round + 35.015), 265);
                break;
        }
        return 0;
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

        //dog rounds
        if (currentWave % 5 == 0)
        {
            isDogRound = true;
        }
        else isDogRound = false;

        isWaveActive = false;
    }

    //hellhound rounds
    private void IsDogRound()
    {

    }

    //rewarding
}
