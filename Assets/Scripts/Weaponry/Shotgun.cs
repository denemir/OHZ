using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{   
    void Start()
    {
        //InstantiateWeapon(transform);
        roundsReloadedPerInstance = 1;

        //if (playerGUIHandler == null)
        //{
        //    Transform currentTransform = transform; // Start from the Weapon's transform
        //                                            // Keep moving up the hierarchy until we either find the PlayerGUIHandler
        //                                            // or reach the top level (null parent).
        //    while (currentTransform != null)
        //    {
        //        playerGUIHandler = currentTransform.GetComponent<PlayerGUIHandler>();
        //        if (playerGUIHandler != null)
        //        {
        //            // We found the PlayerGUIHandler, so we can exit the loop.
        //            break;
        //        }
        //        currentTransform = currentTransform.parent;
        //    }
        //} //gui
    }

    public int numberOfPellets; //number of pellets fired out of each shot

    public float minimumSpread;//minimum spread for each shotgun: shotgun will never fire at perfect accuracy (at least in theory)
 
    //private bool interruptReload { get; set; } //can reload be interrupted by clicking

    public new void Fire(Transform shooterTransform) //override for fire in parent class as to accommodate the multiple pellets fired
    {
        for(int i = 0; i < numberOfPellets; i++)
        {
            base.Fire(shooterTransform);
        }
    }

    //public void FireSinglePellet() //picks a random place within spread range for individual pellet to launch, loop within shotgun fires multiple pellets simultaeneously
    //{
    //    Weapon.Fire();
    //}

    //public new void Reload() //in the case that a shotgun is mag loaded, the reload rate will be slow but rather than reloading the entire magazine, only one (or if pap, then more) round gets reloaded.
    //{
    //    switch(isWeaponHandLoaded)
    //    {
    //        case true: //reload entire magazine like usual.
    //            if (currentStockAmmo > magazineSize)
    //            {
    //                int temp = magazineSize - currentAmmoInMag;
    //                currentAmmoInMag = magazineSize;
    //                currentStockAmmo -= temp;
    //            }
    //            else
    //            {
    //                currentAmmoInMag = currentStockAmmo;
    //                currentStockAmmo = 0;
    //            }
    //            break;

    //            case false: //reload single pellet

    //            if(currentStockAmmo > 0 && currentAmmoInMag < magazineSize)
    //            {
    //                currentStockAmmo--;
    //                currentAmmoInMag++;

    //            }
    //            break;
    //    }
    //}
}
