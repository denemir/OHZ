using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    //details
    public string title;
    public string description;
    public Texture2D icon;

    //map specifications
    //this class can be inherited for map specifics (i.e, create a MapData class for the map itself. it should still be read
    public int numberOfPowerSwitches;
    public SpawnPoints[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
