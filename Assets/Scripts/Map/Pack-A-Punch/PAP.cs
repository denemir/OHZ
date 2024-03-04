using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PAP : Interactable
{
    //interactions
    private Interactable interactable;
    private bool isInitialized = false;
    private Player interactingPlayer;
    private bool isOccupied = false; //is occupied meaning while a player has spun the box and has yet to receive their weapon or let it expire
    public int costToPackAPunch;

    //keybinds
    public KeyCode interactKey;
    private string interactButton;
    public KeyCode giveUpWeaponKey; //key specifically for allowing other players to pick up spun weapon
    private string giveUpWeaponButton;

    //pack a punching the weapon
    public UnityEvent packAPunchWeapon;
    private GameObject packAPunchedWeapon;

    //accepting the weapon
    private bool weaponReady;
    public UnityEvent acceptWeapon; //when the weapon is done being pack a punched

    //player states
    private Dictionary<Player, KeyCode> playerStates = new Dictionary<Player, KeyCode>();

    // Start is called before the first frame update
    void Start()
    {
        //buttons
        interactButton = "Interact";
        giveUpWeaponButton = "AltInteract";

        if (GetComponent<Interactable>() != null)
        {
            interactable = GetComponent<Interactable>();
        }
        else Debug.Log("Interactable component not found on Mysterybox. Please attach Interactable script to Mysterybox Prefab.");

        if (GetComponent<Interactable>().interactions != null)
        {
            InitializeInteractions();
            isInitialized = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //interactions
    private void InitializeInteractions()
    {
        //pack a punch weapon
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to Pack-A-Punch Weapon for " + costToPackAPunch + " points",
            key = interactKey,
            button = interactButton,
            action = packAPunchWeapon,
            holdKeyDown = true,
            holdTime = 1.0f
        });

        //take back pack a punched weapon
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to accept the " +  packAPunchedWeapon.name,
            key = interactKey,
            button = interactButton,
            action = acceptWeapon,
            holdKeyDown = true,
            holdTime = 1.0f
        });

        //occupied
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = ""
        });
    }
    private void SetInteractingPlayer(Player player)
    {
        interactingPlayer = player;
    }
    private void AttemptToInteractWithPackAPunch(Player player)
    {
        if (!isOccupied)
        {
            SetInteractingPlayer(player);
            return;
        }
    }
    private void CheckPackAPunchConditions()
    {
        if (interactingPlayer != null && !isOccupied && HasEnoughPoints(interactingPlayer))
        {
            InitiatePackAPunch();
            return;
        }
    }
    private void InitiatePackAPunch()
    {
        isOccupied = true;
        DeductPoints(interactingPlayer);
        PlayPackAPunchAnimation();

        weaponReady = true;

        //update interaction prompts
        interactable.interactions[2].prompt = "Hold " + interactKey + " to pickup " + packAPunchedWeapon.GetComponent<Weapon>().weaponName;
    }
    //pack a punching
    private void PackAPunchWeapon(Weapon weapon)
    {

    }
    private void TakeBackWeapon()
    {

    }
    private bool HasEnoughPoints(Player player)
    {
        return player.playerStats.points >= costToPackAPunch;
    }
    private void DeductPoints(Player player)
    {

    }
    private void PlayPackAPunchAnimation()
    {

    }
}
