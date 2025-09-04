using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tactical : MonoBehaviour
{
    // info
    public string Name { get; set; }
    public bool IsThrown { get; set; }
    public bool? HasBeenThrown { get; set; }
    public float? Weight { get; set; }
    public int MaxStock { get; set; } // max number of the tacticals able to be held at one time
    public bool HasMultipleUses { get; set; }
    public int? MaxUses { get; set; }

    // timing
    public bool HasLifeTime { get; set; }
    public float? MaxLifeTime { get; set; }
    public float? CurrentLifeTime { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {

    }
}
