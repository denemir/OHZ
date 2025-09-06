using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lethal : MonoBehaviour
{
    // info
    public string Name { get; set; }
    public Sprite Icon { get; set; }

    // stats
    public float MaxDamage { get; set; }
    public float Weight { get; set; }
    public bool IsHolding { get; set; }
    public bool HasCookTime { get; set; }
    public float? MaxCookTime { get; set; }
    private float? CurrentCookTime { get; set; }
    public bool DoesLinger { get; set; }
    public float? MaxLingerTime { get; set; }
    private float? CurrentLingerTime { get; set; }
    public bool DoesExplode { get; set; } // determines if has a blast radius
    public float? BlastRadius { get; set; }
    public GrenadeEffect? GrenadeEffect { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HasCookTime && CurrentCookTime <= 0)
        {
            Explode();
        }
    }

    public virtual void Cook()
    {
        if (HasCookTime)
        {
            CurrentCookTime -= 0.005f;
        }
    }

    public virtual void ResetCookTimer()
    {
        CurrentCookTime = MaxCookTime;
    }

    public virtual void Throw()
    {

    }

    public virtual void Explode()
    {
        //GrenadeEffect.ExecuteEffect();
    }
}
