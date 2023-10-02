using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //weapons
    private int numberOfWeaponSlots = 2; //can only be 3 with a perk
    public Weapon[] weapons;

    private int currentWeaponSlot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //modifying weapons
    public bool AddWeapon(Weapon newWeapon) //when caller is sending to add weapon, if AddWeapon returns false, try SwapWeapon
    {
        int slot = checkForOpenWeaponSlot();
        if (slot != -1)
        {
            weapons[slot] = newWeapon;
            return true;
        }
        return false; //false means no weapon was added
    } //adds weapon in any open slot
    public void DropWeapon(int slot)
    {
        if (weapons[slot] != null)
        {
            weapons[slot].Drop();
            weapons[slot] = null;
        }
    }
    public void SwapWeapon(Weapon newWeapon)
    {
        DropWeapon(currentWeaponSlot);
        weapons[currentWeaponSlot] = newWeapon;
    }

    public void AttachCurrentWeaponToHand()
    { 
        if(GetComponent<Player>().doesCharacterHaveRightHand())
        {
            weapons[currentWeaponSlot].InstantiateWeapon(GetComponent<Player>().GetRightHand().transform);
        }
    }

    private int checkForOpenWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                return i;
            }
        }
        return -1;
    } //if there is an open slot in players inventory, return slot id. return -1 any other case
}
