using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    //players
    public Player[] players;
    private CentralPoint centralPoint;

    //enemies - zombies
    private ZombiePool zombiePool;
    private int activeZombieCount;

    //enemies - hell hounds
    private HellHoundPool hellHoundPool;
    private int activeHellHoundCount;

    //weapons list
    public List<Weapon> weapons;

    //wave
    public WaveHandler waveHandler;

    //points
    public PointHandler pointHandler;

    //map
    public MapData mapData;
    public Map map;

    //states
    public enum DoublePointsState
    { 
        Active,
        Inactive
    }
    private DoublePointsState dps; //DoublePointsState (icydk)
    public enum FireSaleState
    { 
        Active,
        Inactive
    }
    private FireSaleState fireSaleState;
    public enum InstaKillState
    {
        Active,
        Inactive
    }
    private InstaKillState instaKillState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        centralPoint.UpdateCentralPoint();
    }

    //powerups
    public void Nuke() //kaboom.
    {

    }
    public void MaxAmmo()
    {
        foreach(Player player in players)
        {
            //if (player is alive)
            foreach (Weapon weapon in player.GetPlayerInventory().weapons)
            {
                weapon.MaxAmmo();
            }           
        }
    }
    public void Carpenter()
    {

    }
    public void BonusPoints()
    {
        foreach (Player player in players)
        {
            player.playerStats.AddPoints(500); // BONUSS POINTSSSS
        }
    }
    public void BloodMoney()
    {

    }
    public void InitiateFireSale()
    {

    }
    public void EndFireSale()
    {

    }
    public void InitiateDoublePoints()
    {

    }
    public void EndDoublePoints()
    {

    }
    public void InitiateInstaKill()
    {

    }
    public void EndInstaKill()
    {

    }

    //current players
    public void GetAlivePlayers()
    {

    } //returns list of players that are alive (used for spawning and perhaps spectating)
}
