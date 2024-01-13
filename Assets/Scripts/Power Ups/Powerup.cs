using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    //powerup stats
    public string powerupName;
    public string description;
    public bool isInstant;

    //lifetime (time of powerup effects)
    public float maxLifetime;
    private float currentLifetime;

    //pickup lifetime (time before powerup itself despawns)
    public float pickupLifetime;
    private float currentPickupLifetime;

    //model & icon
    public GameObject powerupModel;
    public Sprite icon;

    //interaction
    private void DetectCollision()
    {

    } //check if player intersects collider, if so then apply effects

    //effects
    public void ApplyEffect(MatchHandler match)
    {

    }
    private void DecrementPickupTimer()
    {
        currentPickupLifetime -= 0.005f;
    }
    private void DecrementEffectTimer()
    {

    }
}
