using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminUp : Perk
{
    private float originalPlayerSpeed;
    
    void Start()
    {
        perkName = "StaminUp";
    }

    public override void ApplyPerkEffect(Player player)
    {
        originalPlayerSpeed = player.GetPlayerMovementHandler().moveSpeed;
        player.GetPlayerMovementHandler().moveSpeed = player.GetPlayerMovementHandler().moveSpeed * 1.33f;
    }
}
