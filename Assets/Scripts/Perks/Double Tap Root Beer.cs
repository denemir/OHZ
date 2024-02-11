using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTapRootBeer : Perk
{
    public override void ApplyPerkEffect(Player player)
    {
        player.GetPlayerInventory().SetDoubleTapRootbeerActive();
    }
}
