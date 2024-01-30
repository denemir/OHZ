using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    // Start is called before the first frame update

    //wave details
    public int currentWave;

    //zombies
    public List<int> zombiesRemaining; //zombies remaining in every wave
    public List<int> zombiesKilled; //zombies killed in every wave

    //dogs
    public List<int> hellHoundsRemaining;
    public List<int> hellHoundsKilled;

    //number of players
    private int numberOfPlayersInMatch; //should be passed down from match handler

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
