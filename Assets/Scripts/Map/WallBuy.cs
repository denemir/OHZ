using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WallBuy : MonoBehaviour
{
    //purchasable weapons
    public GameObject weaponObject;
    private Weapon weapon;

    //stats
    public int ammoCost;

    //events
    public UnityEvent onPurchase; //purchase wall buy weapon
    public UnityEvent onAmmoPurchase; //gives player max stock ammo for wall buy weapon if owned


    //interaction 
    public KeyCode interactKey;
    public string interactButton;
    private Interactable interactable;
    private bool isInitialized = false;
    private Player interactingPlayer;

    //player states
    private Dictionary<Player, bool> playerStates = new Dictionary<Player, bool>();

    // Start is called before the first frame update
    void Start()
    {
        weapon = weaponObject.GetComponent<Weapon>();
        interactButton = "Interact";

        if (GetComponent<Interactable>() != null)
        {
            interactable = GetComponent<Interactable>();
        }
        else Debug.Log("Interactable component not found on Wall-buy. Please attach Interactable script to Wall-buy Prefab.");

        if (GetComponent<Interactable>().interactions != null)
        {
            InitializeInteractions();
            isInitialized = true;
        }
        //else Debug.Log("Interactions list not initialized. Please wait");        

    }

    // Update is called once per frame
    void Update()
    {
        if(!isInitialized)
        {
            InitializeInteractions();
            isInitialized = true;
        }

        //check player states
        DeterminePlayerStates();
        if(interactable.getPlayersInRange().Count > 0)
        {
            interactingPlayer = arePlayersInteracting();

            if(interactingPlayer != null)
            {
                InteractWithWallBuy(interactingPlayer);
            }
        }    

        //Updating states
        UpdateInteractionStates();
    }

    //interactions
    private void InitializeInteractions()
    {
        //if weapon is not purchased
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold F to purchase " + weapon.weaponName + " for $" + weapon.cost,
            action = onPurchase,
            key = interactKey,
            button = interactButton,
            holdKeyDown = true,
            holdTime = 1.0f
        });
        /*        Debug.Log("interaction added")*/
        ;

        //if weapon is already purchased
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold F to purchase " + weapon.weaponName + " ammunition for $" + ammoCost,
            action = onAmmoPurchase,
            key = interactKey,
            button = interactButton,
            holdKeyDown = true,
            holdTime = 1.0f
        });

        //default to purchase prompt
        interactable.activeInteraction = interactable.interactions[0];
    }
    private void InteractWithWallBuy(Player player)
    {
        switch (DeterminePlayerState(player))
        {
            case false: //player doesn't have weapon
                //interactable.interactions[0].action.Invoke();
                break;
            case true:
                //interactable.interactions[1].action.Invoke();
                break;
        }

    } /// <summary>
    /// this code can be removed, it is unnecessary as it has been replaced with more modular code (PlayerInteractHandler)
    /// </summary>
    public void PurchaseWeapon()
    {
        if (interactingPlayer.points >= weapon.cost)
        {
            interactingPlayer.points -= weapon.cost;
            if(interactingPlayer.GetPlayerInventory().DoesPlayerHaveAnOpenSlot())
            {
                interactingPlayer.GetPlayerInventory().AddWeapon(weaponObject);
                //Debug.Log("Successfully purchased " + weapon.name);
            }
            else
            {
                interactingPlayer.GetPlayerInventory().SwapWeapon(weaponObject);
                //Debug.Log("Successfully purchased & swapped to " + weapon.name);
            }
        }
    }
    public void PurchaseAmmo()
    {
        if(interactingPlayer.points >= ammoCost)
        {
            //deduct points
            interactingPlayer.points -= ammoCost;

            //add ammo
            interactingPlayer.GetPlayerInventory().weapons[interactingPlayer.GetPlayerInventory().GetMatchingWeaponSlot(weapon)].currentAmmoInMag = weapon.magazineSize;
            interactingPlayer.GetPlayerInventory().weapons[interactingPlayer.GetPlayerInventory().GetMatchingWeaponSlot(weapon)].currentStockAmmo = weapon.maxStockAmmo;
        }
    }
    private void UpdateInteractionStates()
    {
        //determine player prompts
        foreach (Player player in playerStates.Keys) //change per player
        {
            switch(playerStates[player])
            {
                case true:
                    interactable.activeInteraction = interactable.interactions[1]; //set purchase ammo prompt to be active
                    break;
                case false:
                    interactable.activeInteraction = interactable.interactions[0]; //set purchase weapon prompt to be active
                    break;
            }
        }
    } //update which prompt shows

    //player states
    private int DeterminePlayerStates()
    {
        //reset values
        playerStates.Clear();

        //bool hasPlayerPurchasedWeapon = false;
        foreach(Player player in interactable.getPlayersInRange())
        {
            //Determine if player has weapon in their inventory already
            bool hasWeapon = player.GetPlayerInventory().DoesPlayerHaveWeapon(weaponObject);
            playerStates[player] = hasWeapon;
        }
        return 0;
    } //determine if player is within range and has specified weapon
    private bool DeterminePlayerState(Player player)
    {
        return player.GetComponent<PlayerInventory>().DoesPlayerHaveWeapon(weaponObject);
    }
    private Player arePlayersInteracting()
    {
        foreach (Player player in  interactable.getPlayersInRange())
        {
            if(Input.GetKey(interactKey) || Input.GetButton(interactButton))
            {
                return player;
            }
        }
        return null;
    } //determine if any players are interacting with the interactable


}
