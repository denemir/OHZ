using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class MysteryBox : MonoBehaviour
{
    //Mysterybox state
    public enum MysteryBoxState
    {
        Active,
        Inactive
    }
    public MysteryBoxState state;

    //fire sale activates all mystery boxes on map,
    //and all cost 10 playerStats.points.
    public enum FireSaleState
    {
        Active,
        Inactive
    }
    public FireSaleState fireSaleState;

    //stats
    public int costToSpin;

    //available weapons
    public List<GameObject> weaponPrefabs;
    public int weaponCount;
    private double weaponOdds; //determined by 1/weaponCount

    //interactions
    private Interactable interactable;
    private bool isInitialized = false;
    private Player interactingPlayer;
    private Player playerWhoSpun;
    private bool isOccupied = false; //is occupied meaning while a player has spun the box and has yet to receive their weapon or let it expire

    //keybinds
    public KeyCode interactKey;
    private string interactButton;
    public KeyCode giveUpWeaponKey; //key specifically for allowing other players to pick up spun weapon
    private string giveUpWeaponButton;

    //spinning the box
    private bool hasSpun = false;
    private bool doneSpinning;
    public UnityEvent spinBox;
    private GameObject selectedWeaponPrefab;

    //picking up weapon
    public UnityEvent pickUpWeapon; //for player who bought the mystery box
    private string pickUpWeaponPrompt;

    //giving up weapon
    private bool weaponUpForGrabs = false;
    public UnityEvent giveUpWeapon;
    private string giveUpWeaponPrompt;

    //accepting the weapon
    public UnityEvent acceptWeapon; //if the player who purchased the mystery box chose to give up their weapon

    //player states
    private Dictionary<Player, KeyCode> playerStates = new Dictionary<Player, KeyCode>();

    //despawning
    private bool weaponReady = false;
    private float despawnTime; //actual time at which weapon will despawn
    public float timeToDespawn;

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
        //update count to match
        weaponCount = weaponPrefabs.Count;

        if(weaponReady)
        {
            //lower weapon

            if (Time.time > despawnTime)
            {
                CloseLid();
                ResetBoxStatus();
            }
        }

        if (isInitialized)
        {
            //determine player states
            DeterminePlayerStates();

            if (interactable.getPlayersInRange().Count > 0)
            {
                //determine firesale state
                switch (fireSaleState)
                {
                    case FireSaleState.Active:
                        //costToSpin = 10;
                        break;
                    case FireSaleState.Inactive:
                        //costToSpin = 950;
                        break;
                }

                //check for players interacting
                interactingPlayer = ArePlayersInteracting();

                //determine interaction
                if (interactingPlayer != null)
                {
                    InteractWithMysteryBox(interactingPlayer);
                }
            }
        }
        else InitializeInteractions();

        //update prompts
        UpdateInteractionStates();
    }

    //initialization
    private void InitializeInteractions()
    {
        //purchase mystery box
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to spin the mystery box for " + costToSpin + " points",
            key = interactKey,
            button = interactButton,
            action = spinBox,
            holdKeyDown = true,
            holdTime = 1.0f
        }); //Mysterybox Spin prompt

        //pick up weapon
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to pickup weapon or Hold " + giveUpWeaponKey + " to let other players take it",
            key = interactKey, //the keys to giveUpWeapon and interact are swapped intentionally. this is to prevent picking up and spinning the box at the same time.
            button = interactButton,
            hasAltEvent = true,
            altKey = giveUpWeaponKey,
            altButton = giveUpWeaponButton,
            action = pickUpWeapon,
            altAction = giveUpWeapon,
            holdKeyDown = true,
            holdTime = 0.8f
        });

        //let other players take weapon
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to pickup weapon",
            key = interactKey,
            button = interactButton,
            action = acceptWeapon,
            holdKeyDown = true,
            holdTime = 0.8f
        });

        //empty interaction for players watching
        interactable.interactions.Add(new Interactable.Interaction());

        //set initial interaction
        interactable.activeInteraction = interactable.interactions[0];
        isInitialized = true;
    }

    //interacting
    private void InteractWithMysteryBox(Player player)
    {
        if (!hasSpun/* || playerWhoSpun == null*/)
        {
            SetPlayerWhoSpun(player);
            //interactable.Interact(interactable.interactions[0], player);//spin box
            //interactable.interactions[0].action.Invoke(); //spin box
            return;
        }
        //else if (doneSpinning && playerStates[player] == interactKey && player == playerWhoSpun) //if player was who spun the box, decide
        //{
        //    //interactable.interactions[1].action.Invoke(); //pick up weapon
        //    return;
        //}
        //else if (doneSpinning && playerStates[player] == giveUpWeaponKey && player == playerWhoSpun)
        //{
        //    //interactable.interactions[1].altAction.Invoke(); //give up weapon
        //    return;
        //}

        //if (weaponUpForGrabs && playerStates[player] == interactKey)
        //{
        //    //interactable.interactions[2].action.Invoke(); //accept weapon
        //    return;
        //}
    }
    private Player ArePlayersInteracting()
    {
        foreach (Player player in interactable.getPlayersInRange())
        {
            switch(player.inputState)
            {
                case Player.InputState.KandM:
                    if (Input.GetKey(interactKey) && !isOccupied)
                    {
                        interactingPlayer = player;

                        return player;
                    }
                    else if (Input.GetKey(interactKey) && doneSpinning)
                    {
                        interactingPlayer = player;
                        playerStates[player] = interactKey;
                        return player;
                    }
                    else if (Input.GetKey(giveUpWeaponKey) && doneSpinning)
                    {
                        interactingPlayer = player;
                        playerStates[player] = giveUpWeaponKey;
                        return player;
                    }
                    break;

                    case Player.InputState.Controller:
                    if (Input.GetButton(interactButton) && !isOccupied)
                    {
                        interactingPlayer = player;

                        return player;
                    }
                    else if (Input.GetButton(interactButton) && doneSpinning)
                    {
                        interactingPlayer = player;
                        playerStates[player] = interactKey; // i just realized the dict takes keycodes... oops! dupe code
                        return player;
                    }
                    else if (Input.GetButton(giveUpWeaponButton) && doneSpinning)
                    {
                        interactingPlayer = player;
                        playerStates[player] = giveUpWeaponKey;
                        return player;
                    }
                    break;
            }
         
        }
        return null;
    } //determine if any players are interacting with the interactable
    private void UpdateInteractionStates()
    {
        foreach (Player player in playerStates.Keys)
        {
            //check if box has been spun
            if(hasSpun)
            {
                //if player is the one who spun the box
                if(player == playerWhoSpun)
                {
                    if (doneSpinning && !weaponUpForGrabs)
                        interactable.activeInteraction = interactable.interactions[1]; //prompt to pick up
                    else if(doneSpinning)
                        interactable.activeInteraction = interactable.interactions[2]; //prompt to pick up
                    else interactable.activeInteraction = interactable.interactions[3]; //blank
                }
                else
                {
                    switch(weaponUpForGrabs)
                    {
                        case true:
                            interactable.activeInteraction = interactable.interactions[2]; //prompt to accept weapon
                            break;
                        case false:
                            interactable.activeInteraction = interactable.interactions[3]; //blank
                            break;
                    }
                }
            } else interactable.activeInteraction = interactable.interactions[0]; //prompt to spin
                       
        }
    }
    private int DeterminePlayerStates()
    {
        //reset values
        playerStates.Clear();

        foreach (Player player in interactable.getPlayersInRange())
        {
            //Determine if player has pressed any button
            playerStates[player] = 0;
        }
        return 0;
    } //determine if player is within range and has specified weapon

    //pre-spin
    private bool HasEnoughPoints(Player player)
    {
        if (player.playerStats.points >= costToSpin)
            return true;
        return false;
    }
    public bool isCurrentlyOccupied()
    {
        return isOccupied;
    }

    //spin
    public void CheckSpinConditions()
    {
        if (playerWhoSpun != null && HasEnoughPoints(playerWhoSpun) && !isOccupied && !hasSpun)
        {
            InitiateSpin();
            //interactable.interactions[0].player = null;
        }
        //reset interacting player
        //interactingPlayer = null;
    }
    private void InitiateSpin()
    {
        hasSpun = true;
        isOccupied = true;
        DeductPoints(playerWhoSpun);
        PlaySpinAnimation();
        DetermineWeapon();

        weaponReady = true;
        despawnTime = Time.time + timeToDespawn;

        //update interaction prompts
        interactable.interactions[1].prompt = "Hold " + interactKey + " to pickup " + selectedWeaponPrefab.GetComponent<Weapon>().weaponName + " or Hold " + giveUpWeaponKey + " to let other players take it";
        interactable.interactions[2].prompt = "Hold " + interactKey + " to pickup " + selectedWeaponPrefab.GetComponent<Weapon>().weaponName;
    }
    private void DeductPoints(Player player)
    {
        player.playerStats.points -= costToSpin;
    }
    private void DetermineWeapon()
    {        
        //select weapon
        GameObject selected = weaponPrefabs[Random.Range(0, weaponCount)];

        //check to make sure player does not have weapon already several times
        int attempts = 0;
        while(playerWhoSpun.GetPlayerInventory().DoesPlayerHaveWeapon(selected) && attempts < 20)
        {
            //Debug.Log("Try " + attempts);
            selected = weaponPrefabs[Random.Range(0, weaponCount)];
            attempts++;
        }

        //assign weapon to prefab
        selectedWeaponPrefab = selected;
    }
    private void PlaySpinAnimation() //cycles thru all possible weapons from mystery box
    {
        //needs a cycling weapon object
        doneSpinning = true;
    }
    private void PlayMysteryBoxJingle()
    {

    }
    private void PlaySpecialEffect()
    {

    }

    //post-spin
    public void GiveUpWeapon()
    {
        //Debug.Log("Weapon has been given up");
        weaponUpForGrabs = true;
    } //allow other players to pickup weapon
    public void PickUpWeapon()
    {
        if (doneSpinning && selectedWeaponPrefab != null)
        {
            //Debug.Log("Player " + interactingPlayer.name + " picked up the " + selectedWeaponPrefab.GetComponent<Weapon>().weaponName + ".");
            if (interactingPlayer.GetPlayerInventory().DoesPlayerHaveAnOpenSlot())
            {
                interactingPlayer.GetPlayerInventory().AddWeapon(selectedWeaponPrefab);
                ResetBoxStatus();
            }
            else
            {
                interactingPlayer.GetPlayerInventory().SwapWeapon(selectedWeaponPrefab);
                ResetBoxStatus();
            }
        }

    } //pick up weapon from box
    private void CloseLid()
    {

    } //closes the mystery box
    private Player GetPlayerWhoSpun()
    {
        return playerWhoSpun;
    } //get the player who spun the box
    private void SetPlayerWhoSpun(Player player)
    {
        playerWhoSpun = player;
    } //set the player who spun the box
    private void ResetBoxStatus()
    {
        //Debug.Log("box has been reset");
        weaponUpForGrabs = false;
        isOccupied = false;
        hasSpun = false;
        doneSpinning = false;
        selectedWeaponPrefab = null;
        interactingPlayer = null;
        playerWhoSpun = null;
        weaponReady = false;

        //reset interactable
        interactable.activeInteraction = interactable.interactions[0];
    } // for after picking up weapon
}
