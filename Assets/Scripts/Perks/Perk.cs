using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour
{
    //perk stats
    public string perkName;
    public string perkDescription;
    public Sprite icon;

    //perk effects
    public virtual void ApplyPerkEffect()
    {

    }
    public virtual void ApplyPerkEffect(Player player)
    {

    }
}
