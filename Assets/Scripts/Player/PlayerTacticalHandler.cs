using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTacticalHandler : MonoBehaviour
{
    public List<Tactical> Tacticals { get; private set; }
    private int MaxCount { get; set; }
    private Player Player { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddTactical(Tactical tactical)
    {
        if (Tacticals.Count < MaxCount)
        {
            Tacticals.Add(tactical);
        }
    }

    public void RemoveTactical()
    {
        Tacticals.RemoveAt(Tacticals.Count - 1);
    }

    public void Use()
    {
        Tacticals[Tacticals.Count - 1].Use(Player);
        RemoveTactical();
    }

    public void Throw()
    {
        RemoveTactical();
    }

    public void Hold()
    {

    }
}
