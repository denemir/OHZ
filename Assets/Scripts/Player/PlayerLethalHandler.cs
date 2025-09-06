using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLethals : MonoBehaviour
{
    public List<Lethal> Lethals { get; private set; }
    private int MaxCount { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddLethal(Lethal Lethal)
    {
        if(Lethals.Count < MaxCount)
        {
            Lethals.Add(Lethal);
        }
    }

    public void RemoveLethal()
    {
        Lethals.RemoveAt(Lethals.Count - 1);
    }

    public void Throw()
    {
        RemoveLethal();
    }

    public void Hold()
    {
        Lethals[Lethals.Count - 1].Cook();
    }
}
