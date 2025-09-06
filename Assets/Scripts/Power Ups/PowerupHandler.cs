using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupHandler : MonoBehaviour
{
    public MatchHandler MatchHandler { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        MatchHandler = GetComponent<MatchHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MatchHandler == null) MatchHandler = GetComponent<MatchHandler>();
    }

    public void TriggerPowerupForPlayer(Powerup powerup, Player player)
    {
        powerup.ApplyEffectToPlayer(player);
    }

    public void TriggerPowerupForMatch(Powerup powerup)
    {
        powerup.ApplyEffectToAllPlayers(MatchHandler);
    }
}
