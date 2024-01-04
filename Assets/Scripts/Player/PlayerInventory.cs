using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //weapons
    private int numberOfWeaponSlots/* = 2*/; //can only be 3 with a perk
    public GameObject[] weaponPrefabs;
    public Weapon[] weapons;

    public Weapon activeWeapon;
    private GameObject currentWeaponInstance;

    public int currentWeaponSlot;
    private bool hasWeaponSpawned = false;

    private float swapWeaponTimer = 0;

    //debug
    public DebugHandler debugHandler;
    public bool isDebuggingOn;

    // Start is called before the first frame update
    void Start()
    {
        currentWeaponSlot = 0;
        if (weaponPrefabs[currentWeaponSlot] != null) //spawn current weapon
            SpawnWeapon(currentWeaponSlot);

        //debug
        if (debugHandler != null && debugHandler.IsDebuggingOn())
            isDebuggingOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasWeaponSpawned)
        {

        }

        if (!IsSwapWeaponTimerZero())
            swapWeaponTimer--;

        //UpdateWeaponsArray();
    }

    //modifying weapons
    public bool AddWeapon(GameObject newWeaponPrefab) //when caller is sending to add weapon, if AddWeapon returns false, try SwapWeapon
    {
        int slot = CheckForOpenWeaponSlot();
        if (slot != -1)
        {
            weaponPrefabs[slot] = newWeaponPrefab;
            SwapCurrentWeapon(slot);

            if (isDebuggingOn)
                Debug.Log("Weapon added to slot " + slot);
            return true;
        }

        if (isDebuggingOn)
            Debug.Log("No open weapon slots found.");

        return false; //false means no weapon was added
    } //adds weapon in any open slot
    public void HideWeapon(int slot)
    {
        if (isDebuggingOn)
            Debug.Log("Hiding weapon slot " + slot);

        if (weaponPrefabs[slot] != null)
        {
            weapons[slot].isActive = false;
            weapons[slot] = null;
        }
    }
    public void DropWeapon(int slot)
    {
        if (isDebuggingOn)
            Debug.Log("Dropping weapon slot " + slot);
        if (weaponPrefabs[slot] != null)
        {
            weapons[slot].Drop();
            weapons[slot] = null;
        }
    }
    public void SwapWeapon(GameObject newWeapon)
    {
        //replacing weapon with purchased weapon
        DropWeapon(currentWeaponSlot);
        weaponPrefabs[currentWeaponSlot] = newWeapon;

        //spawning & setting it to be active
        SpawnWeapon(currentWeaponSlot);

    } //swap weapons
    public void SwapCurrentWeapon(int slot)
    {
        if (weaponPrefabs[slot] == null) 
        {
            //Debug.LogError("Weapon slot does not contain a weapon.");
            return;
        }

        //set current weapon to be inactive
        if(currentWeaponInstance != null)
        {
            currentWeaponInstance.SetActive(false);
            //Debug.Log("" + currentWeaponInstance.name + " set inactive");
        }

        //setting new weapon
        if (weapons[slot] == null) //if weapon doesn't exist, spawn an instance of it.
            SpawnWeapon(slot);
        else
        {
            weapons[currentWeaponSlot].gameObject.SetActive(false);
            weapons[slot].gameObject.SetActive(true);
            activeWeapon = weapons[slot];
            //Debug.Log("Spawned wepaon from inventory, weapon slot " + slot);
        }

        //set current weapon slot to selected slot
        currentWeaponSlot = slot;

        //total swap time accounts for both weapons
        swapWeaponTimer += weapons[currentWeaponSlot].weaponSwapTime + weapons[slot].weaponSwapTime;
    } //swap current weapon based on inventory slots
    public void CycleCurrentWeapon() //for controllers to switch weapons!
    {
        //check if weapon is the last in the list, if not then increment weapon slot within list
        if(currentWeaponSlot != weaponPrefabs.Length - 1 && weaponPrefabs[currentWeaponSlot + 1] != null)
        {
            SwapCurrentWeapon(currentWeaponSlot + 1);
        } else
        {
            //otherwise, just set the weapon slot back to 0
            SwapCurrentWeapon(0);
        }
    }
    public void AttachCurrentWeaponToHand(GameObject weaponModel)
    {
        if (GetComponent<Player>().DoesCharacterHaveRightHand())
        {
            //Transform rightHandTransform = GetComponent<Player>().GetRightHand().transform;
            //weaponModel.transform.SetParent(rightHandTransform);
            Transform rightHandTransform = GetComponent<Player>().GetRightHand().transform;
            currentWeaponInstance.transform.parent = rightHandTransform; // Set the parent
            currentWeaponInstance.transform.localPosition = Vector3.zero; // Set the position
            currentWeaponInstance.transform.localRotation = Quaternion.identity; // Set the rotation

            hasWeaponSpawned = true;
        }
        else Debug.Log("Player right hand does not exist.");
    }


    //spawning weapons
    public void SpawnWeapon(int slot)
    {
        if (weaponPrefabs[slot] != null)
        {
            if(currentWeaponInstance != null)
            {
                currentWeaponInstance.SetActive(false);
            }

            currentWeaponInstance = Instantiate(weaponPrefabs[slot]);
            Weapon weapon = /*weaponPrefabs[slot]*/currentWeaponInstance.GetComponent<Weapon>();

            if (GetComponent<Player>().DoesCharacterHaveRightHand())
            {
                //spawn
                AttachCurrentWeaponToHand(/*Instantiate(weaponPrefabs[slot])*/currentWeaponInstance);
                hasWeaponSpawned = true;

                //set activeWeapon
                activeWeapon = weapon;

                //set components
                activeWeapon.SetBarrelTip(currentWeaponInstance.GetComponentInChildren<BarrelTip>());

                //add to weapons list
                weapons[slot] = weapon;
            }
            else Debug.Log("Player right hand does not exist.");

        }
    } //spawn weapon from slot
    public GameObject SpawnWeaponInstance(GameObject weapon, int slot)
    {
            GameObject tempWeaponInstance = Instantiate(weapon);
            Weapon weaponScript = /*weaponPrefabs[slot]*/tempWeaponInstance.GetComponent<Weapon>();

            if (GetComponent<Player>().DoesCharacterHaveRightHand())
            {
                //spawn
                AttachCurrentWeaponToHand(/*Instantiate(weaponPrefabs[slot])*/currentWeaponInstance);
                hasWeaponSpawned = true;

                //set activeWeapon
                //activeWeapon = weaponScript;

                //set components
                weaponScript.SetBarrelTip(tempWeaponInstance.GetComponentInChildren<BarrelTip>());

                //add to weapons list
                weapons[slot] = weaponScript;
            return tempWeaponInstance;
            }
            else Debug.Log("Player right hand does not exist.");
        return null;
    }    


    //use weapons
    public Weapon GetCurrentWeapon()
    {
        //Debug.Log("Returning");
        return activeWeapon;
    }
    public int GetCurrentWeaponSlot()
    {
        return currentWeaponSlot;
    }
    public int GetMatchingWeaponSlot(Weapon keyWeapon)
    {
        int i = 0;
        foreach(var weapon in weapons)
        {
            if(weapon.weaponName == keyWeapon.weaponName)
            {
                return i;
            }
            i++;
        }
        return -1;
    } //for adding ammo
    public bool DoesPlayerHaveWeapon(GameObject targetWeapon)
    {
        if(weaponPrefabs[0] != null)
        {
            return weaponPrefabs.Contains(targetWeapon);
            //foreach (GameObject weapon in weaponPrefabs)
            //{
            //    if (weapon.GetComponent<Weapon>().weaponName == targetWeapon.weaponName)
            //        return true;
            //}
        }
        
        return false;
    }


    //swap weapon time
    public bool IsSwapWeaponTimerZero()
    {
        return (swapWeaponTimer == 0);
    }
    public void AddToSwapWeaponTimer(float time)
    {
        swapWeaponTimer += time;
    }

    //inventory space
    public bool DoesPlayerHaveAnOpenSlot()
    {
        if (CheckForOpenWeaponSlot() != -1)
            return true;
        return false;
    } //returns if player has an open slot
    private int CheckForOpenWeaponSlot()
    {
        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            if (weapons[i] == null)
            {
                return i;
            }
        }
        return -1;
    } //if there is an open slot in players inventory, return slot id. return -1 any other case

}
