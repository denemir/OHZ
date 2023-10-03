using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour
{
    // profile is used for storing the players multiplayer information, as well as potentially storing guest profiles
    public string userName;

    public Color favoriteColor;

    //stats
    public int totalKills;
    public int totalDeaths;
    public float kdRatio/* = (float)totalKills / (float)totalDeaths*/;

    public int downs;
    public int revives;

    public int matchesPlayed;
    public int matchesQuit;
    public int altF4s; //as it sounds.

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
