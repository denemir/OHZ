using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    //vars
    public List<GameObject> currentlyTargeting;
    public List<GameObject> currentlyTargetedBy;
    private bool canBeTargeted;

    //updating lists
    public void AddToCurrentlyTargeting(GameObject target)
    {
        currentlyTargeting.Add(target);
    }
    public void AddToCurrentlyTargetedBy(GameObject targeter)
    {
        currentlyTargeting.Add(targeter);
    }

    public List<GameObject> GetCurrentlyTargeting()
    { return  currentlyTargeting; }
    public List<GameObject> GetCurrentlyTargetedBy() 
    {  return currentlyTargetedBy; }

    public void ClearAllCurrentlyTargetedBy() //in the case that the player dies or the barricade is broken, return to the targeter that the object cannot be targeted
    {

    }
    public bool CanBeTargeted()
    {
        return canBeTargeted;
    }
}
