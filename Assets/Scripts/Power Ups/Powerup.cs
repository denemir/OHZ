using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // powerup stats
    public string Name { get; set; }
    public bool IsInstant { get; set; }
    public bool IsDrop { get; set; }
    public bool IsDropped { get; set; }
    public bool IsCurrentlyActive { get; private set; }

    // lifetime (time of powerup effects)
    public float MaxActiveLifetime;
    private float CurrentActiveLifetime;
    public float PickupLifetime;
    private float CurrentPickupLifetime;

    // model & icon
    public Sprite icon;

    private void FixedUpdate()
    {
        if (IsCurrentlyActive) DecrementEffectTimer();
        if (IsDropped) DecrementPickupTimer();
    }

    // interaction
    private void DetectCollision()
    {

    }

    // effects
    public virtual void ApplyEffectToAllPlayers(MatchHandler match)
    {

    }
    public virtual void ApplyEffectToPlayer(Player player)
    {

    }

    private void DecrementPickupTimer()
    {
        CurrentPickupLifetime -= 0.005f;
    }
    private void DecrementEffectTimer()
    {
        CurrentActiveLifetime -= 0.005f;
    }
}
