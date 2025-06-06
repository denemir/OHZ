using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    //players
    public List<Player> players;
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

    //barricades
    private int barricadesCurrentlyOpen;

    //states
    public enum DoublePointsState
    { 
        Active,
        Inactive
    }
    private DoublePointsState dps; // whether or not double points is active
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
        //centralPoint.UpdateCentralPoint();
        if(players.Count > 0)
        {

        }
    }

    //powerups
    public void Nuke() //kaboom.
    {
        //zombiePool.NukeAllZombies();
    }
    public void MaxAmmo()
    {
        foreach(Player player in players)
        {
            //(if player is alive)
            foreach (Weapon weapon in player.GetPlayerInventory().weapons)
            {
                weapon.MaxAmmo();
            }           
        }
    }
    public void Carpenter()
    {

    } //repair all barricades
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
    public void UpdatePlayerGUIWaveCounter(int currentWave)
    {
        foreach(Player player in players)
        {
            player.GetComponent<PlayerGUIHandler>().UpdateCurrentWave(currentWave);
        }
    }
}
