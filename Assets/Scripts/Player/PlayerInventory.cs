using System.Collections;
using System.Collections.Generic;
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

    private int currentWeaponSlot;
    private bool hasWeaponSpawned = false;

    private float swapWeaponTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentWeaponSlot = 0;
        if (weaponPrefabs[currentWeaponSlot] != null)
            SpawnWeapon(currentWeaponSlot);
        UpdateWeaponsArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasWeaponSpawned)
        {

        }

        if (!isSwapWeaponTimerZero())
            swapWeaponTimer--;

        UpdateWeaponsArray();
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
        SpawnWeapon(currentWeaponSlot);
    } //swap weapons
    public void SwapCurrentWeapon(int slot)
    {
        if (weaponPrefabs[slot] == null) 
        {
            Debug.LogError("Weapon slot does not contain a weapon.");
            return;
        }

        if(currentWeaponInstance != null)
        {
            currentWeaponInstance.SetActive(false);
        }

        ////Despawn current weapon
        //DespawnCurrentWeapon(currentWeaponSlot);

        //checking original weapon properties (if they exist)
        Weapon stats = null;
        if (weapons[slot] != null)
        {
            stats = weapons[slot];
        }

        //setting new weapon
        SpawnWeapon(slot);

        if(stats != null)
        {
            weapons[currentWeaponSlot] = stats;
            activeWeapon = stats;
        }

        //total swap time accounts for both weapons
        swapWeaponTimer += weapons[currentWeaponSlot].weaponSwapTime + weapons[slot].weaponSwapTime;
    } //swap current weapon based on inventory slots
    public bool DoesPlayerHaveWeapon(Weapon targetWeapon)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.weaponName == targetWeapon.weaponName)
                return true;
        }
        return false;
    }

    //public void AttachCurrentWeaponToHand()
    //{
    //    if (GetComponent<Player>().doesCharacterHaveRightHand())
    //    {
    //        Transform rightHandTransform = GetComponent<Player>().GetRightHand().transform;

    //        weapons[currentWeaponSlot].InstantiateWeapon(rightHandTransform);
    //    }
    //    else Debug.Log("Player right hand does not exist.");
    //}
    public void AttachCurrentWeaponToHand(GameObject weaponModel)
    {
        if (GetComponent<Player>().doesCharacterHaveRightHand())
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

    private void UpdateWeaponsArray()
    {
        int i = 0;
        foreach(GameObject prefab in weaponPrefabs)
        {
            if (weapons[i] == null)
            {
                SpawnWeaponInstance(prefab, i);
                //DespawnWeapon(prefab, i);
                //weapons[i] = prefab.GetComponent<Weapon>();
            }

            //increment
            i++;
        }
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

            if (GetComponent<Player>().doesCharacterHaveRightHand())
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
    }

    public GameObject SpawnWeaponInstance(GameObject weapon, int slot)
    {
            GameObject tempWeaponInstance = Instantiate(weapon);
            Weapon weaponScript = /*weaponPrefabs[slot]*/tempWeaponInstance.GetComponent<Weapon>();

            if (GetComponent<Player>().doesCharacterHaveRightHand())
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
    //public void DespawnCurrentWeapon(int currentSlot)
    //{
    //    if (weapons[currentSlot] != null && activeWeapon != null)
    //    {
    //        weapons[currentWeaponSlot] = activeWeapon;
    //        ////copying properties to inventory
    //        //Weapon wi = currentWeaponInstance.GetComponent<Weapon>();
    //        //weapons[currentSlot] = Instantiate(wi);

    //        //destroying the evidence
    //        Destroy(currentWeaponInstance);
    //        activeWeapon = null;
    //    }
    //}
    //public void DespawnWeapon(GameObject weapon, int slot)
    //{
    //    if (weapons[slot] != null)
    //    {
    //        weapons[slot] = weapon.GetComponent<Weapon>();
    //        ////copying properties to inventory
    //        //Weapon wi = currentWeaponInstance.GetComponent<Weapon>();
    //        //weapons[currentSlot] = Instantiate(wi);

    //        //destroying the evidence
    //        Destroy(weapon);
    //    }
    //}

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

    //swap weapon time
    public bool isSwapWeaponTimerZero()
    {
        return (swapWeaponTimer == 0);
    }

    public void addToSwapWeaponTimer(float time)
    {
        swapWeaponTimer += time;
    }
}
