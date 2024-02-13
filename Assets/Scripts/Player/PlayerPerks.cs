using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerks : MonoBehaviour
{
    public List<Perk> activePerks = new List<Perk>();
    public int perkMaximum;
    private int currentNumOfPerks;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //modifying perks
    public void AddPerk(Perk perk)
    {
        if(!activePerks.Contains(perk) && activePerks.Count <= perkMaximum)
        {
            activePerks.Add(perk);
            perk.ApplyPerkEffect(player);
        }
    }
    public void LosePerk(Perk perk)
    {

    }
    public void LoseRandomPerk()
    {

    }
    public Perk GetPerk(int slot)
    { 
        if(slot < activePerks.Count)
        return activePerks[slot];
        return null;
    }

    //animations
    public void DrinkPerkSodaAnimation()
    {

    }

    //checks
    public bool DoesPlayerHavePerk(Perk perk)
    {
        return activePerks.Contains(perk);
    }
    public bool DoesPlayerHavePerk(string name)
    {
        foreach(Perk perk in activePerks)
        {
            if (perk.perkName.ToLower() == name.ToLower())
                return true;
        }
        return false;
    }
}
