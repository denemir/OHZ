using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackAPunchedWeapon : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        roundsReloadedPerInstance = 2;
        timeBetweenShots = 1 / (roundsPerMinute / 60f);
    }
}
