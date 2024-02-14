using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juggernog : Perk
{
    // Start is called before the first frame update
    void Start()
    {
        perkName = "Juggernog"; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ApplyPerkEffect(Player player)
    {
        player.playerStats.AddMaxHealth(100);
    }
}
