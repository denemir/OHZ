using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    // info
    public string Name { get; set; }
    public Sprite Sprite { get; set; }

    // interactability
    public bool IsDrop { get; set; }
    public bool HasPickupTime { get; set; } // whether the pickup is a static spawn or a drop
    public bool IsActive { get; set; } // whether the collectable has been picked up and the timer is running
    public float MaxPickupTime { get; set; }
    public float RemainingPickupTime { get; set; }
    public float MaxActiveTime { get; set; }
    public float RemainingActiveTime { get; set; }
    public bool IsColliding { get; set; }
    public UnityEvent action { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        IsActive = false;
        RemainingActiveTime = 0;
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    private void FixedUpdate()
    {
        if (IsActive && RemainingActiveTime > 0.0)
        {
            RemainingActiveTime -= 0.05f;
        }

        if (IsDrop && !IsActive && RemainingPickupTime > 0.0)
        {
            RemainingPickupTime -= 0.05f;
        }
    }

    // Collectable Specific Functions
    public virtual void Collect()
    {
        action.Invoke();
        IsActive = true;
        RemainingActiveTime = MaxActiveTime;
        RemainingPickupTime = 0;
    }

    public virtual void Spawn()
    {

    }

    public virtual void Despawn()
    {

    }
}
