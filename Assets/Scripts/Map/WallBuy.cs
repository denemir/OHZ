using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WallBuy : MonoBehaviour
{
    //purchasable weapons
    public Weapon weapon;

    //events
    public UnityEvent onPurchase; //purchase wall buy weapon
    public UnityEvent onAmmoPurchase; //gives player max stock ammo for wall buy weapon if owned

    public KeyCode interactKey;

    //interaction 
    private Interactable interactable;
    private bool isInitialized = false;

    //player states
    private Dictionary<Player, bool> playerStates = new Dictionary<Player, bool>();

    // Start is called before the first frame update
    void Start()
    {
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
        DeterminePlayerState();

        //Updating states
        UpdateInteractionStates();
    }

    //interactions
    public void PurchaseWeapon()
    {

    }

    public void PurchaseAmmo()
    {
        
    }


    private void InitializeInteractions()
    {
        //if weapon is not purchased
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold F to purchase " + weapon.weaponName + " for $" + weapon.cost,
            action = onPurchase,
            key = interactKey

        });
/*        Debug.Log("interaction added")*/;

        //if weapon is already purchased
        interactable.interactions.Add(new Interactable.Interaction
        { 
            prompt = "Hold F to purchase " + weapon.weaponName + " ammunition for $500",
            action = onAmmoPurchase,
            key = interactKey

        });

        //default to purchase prompt
        interactable.activeInteraction = interactable.interactions[0];
    }

    private void UpdateInteractionStates()
    {
        //determine player prompts
        foreach (Player player in playerStates.Keys) //change per player
        {
            if (playerStates[player])
            {
                interactable.activeInteraction = interactable.interactions[1]; //set purchase ammo prompt to be active
            }
            else interactable.activeInteraction = interactable.interactions[0]; //set purchase weapon prompt to be active
        }
    } //update which prompt shows

    //player states
    private int DeterminePlayerState()
    {
        //reset values
        playerStates.Clear();

        //bool hasPlayerPurchasedWeapon = false;
        foreach(Player player in interactable.getPlayersInRange())
        {
            //Debug.Log("Checking player " + player.playerName);

            //Determine if player has weapon in their inventory already
            bool hasWeapon = player.GetPlayerInventory().DoesPlayerHaveWeapon(weapon);
            playerStates[player] = hasWeapon;
        }
        return 0;
    }
    private int DeterminePlayerState(Player player)
    {
        return 0;
    }

}
