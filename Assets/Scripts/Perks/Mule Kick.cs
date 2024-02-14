using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuleKick : Perk
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ApplyPerkEffect(Player player)
    {
        player.GetPlayerInventory().SetMuleKickActive();
    }
}
