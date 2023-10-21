using System.Collections;
using System.Collections.Generic;
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
    }

    public void PurchaseWeapon()
    {

    }

    private void InitializeInteractions()
    {
        //if weapon is not purchased
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold F to purchase " + weapon.name + " for $" + weapon.cost,
            action = onPurchase,
            key = interactKey

        });
/*        Debug.Log("interaction added")*/;

        //if weapon is already purchased
        interactable.interactions.Add(new Interactable.Interaction
        { 
            prompt = "Hold F to purchase " + weapon.name + " ammunition for $500",
            action = onAmmoPurchase,
            key = interactKey

        });

        interactable.activeInteraction = interactable.interactions[0]; //set purchase weapon prompt to be active
        //Debug.Log("interaction set as active");
    }

    private int DeterminePlayerState()
    {
        //bool hasPlayerPurchasedWeapon = false;
        foreach(Player player in interactable.getPlayersInRange())
        {

            //switch (hasPlayerPurchasedWeapon)
            //{ 
                
            //}

        }
        return 0;
    }

    private int DeterminePlayerState(Player player)
    {
        return 0;
    }

}
