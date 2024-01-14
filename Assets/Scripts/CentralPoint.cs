using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralPoint : MonoBehaviour
{
    //match
    public MatchHandler matchHandler; //should be parent of centralpoint
    public List<Player> activePlayers;

    /// <summary>
    /// CentralPoint class serves as a way to help track the centermost place of amongst all of the existing players, in which 
    /// the point can be used for determining the potential locations to spawn.
    /// </summary>

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateActivePlayers(List<Player> activePlayers)
    {
        this.activePlayers = activePlayers;
    }
    public void UpdateCentralPoint()
    {
        Vector3 sum = Vector3.zero;
        foreach (Player player in activePlayers)
        {
            sum += player.transform.position;
        }
        transform.position = sum / activePlayers.Count;
    }
    public Transform GetCentralPoint()
    { return transform; }
}
