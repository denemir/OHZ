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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
