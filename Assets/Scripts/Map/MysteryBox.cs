using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerUnityEvent : UnityEvent<Player>
{
}

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
    //and all cost 10 points.
    public enum FireSaleState
    {
        Active,
        Inactive
    }
    public FireSaleState fireSaleState;

    //stats
    public int costToSpin;

    //keybinds
    public KeyCode interactKey;
    public KeyCode giveUpWeaponKey; //key specifically for allowing other players to pick up spun weapon

    //available weapons
    public List<GameObject> weaponPrefabs;
    public int weaponCount;
    private double weaponOdds; //determined by 1/weaponCount

    //interactions
    private Interactable interactable;
    private bool isInitialized = false;
    public Player interactingPlayer;
    private Player playerWhoSpun;
    private bool isOccupied = false; //is occupied meaning while a player has spun the box and has yet to receive their weapon or let it expire

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


    // Start is called before the first frame update
    void Start()
    {
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
                        costToSpin = 10;
                        break;
                    case FireSaleState.Inactive:
                        costToSpin = 950;
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
            action = spinBox
        }); //Mysterybox Spin prompt

        //pick up weapon
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to pickup weapon or Hold " + giveUpWeaponKey + " to let other players take it",
            key = interactKey,
            altKey = giveUpWeaponKey,
            action = pickUpWeapon,
            altAction = giveUpWeapon
        });

        //let other players take weapon
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold " + interactKey + " to pickup weapon",
            key = interactKey,
            action = acceptWeapon
        });

        //empty interaction for players watching
        interactable.interactions.Add(new Interactable.Interaction());

        interactable.activeInteraction = interactable.interactions[0];
    }

    //interacting
    private void InteractWithMysteryBox(Player player)
    {
        if (!hasSpun || playerWhoSpun == null)
        {
            SetPlayerWhoSpun(player);
            interactable.Interact(interactable.interactions[0], player);//spin box
            //interactable.interactions[0].action.Invoke(); //spin box
            Debug.Log("money");
            return;
        }
        else if (doneSpinning && playerStates[player] == interactKey && player == playerWhoSpun) //if player was who spun the box, decide
        {
            interactable.interactions[1].action.Invoke(); //pick up weapon
            return;
        }
        else if (doneSpinning && playerStates[player] == giveUpWeaponKey && player == playerWhoSpun)
        {
            interactable.interactions[1].altAction.Invoke(); //give up weapon
            return;
        }

        if (weaponUpForGrabs && playerStates[player] == interactKey)
        {
            interactable.interactions[2].action.Invoke(); //accept weapon
            return;
        }
    }
    private Player ArePlayersInteracting()
    {
        foreach (Player player in interactable.getPlayersInRange())
        {
            Debug.Log("Nut");
            if (Input.GetKeyDown(interactKey) && !isOccupied)
            {
                Debug.Log("Wtf");
                interactingPlayer = player;
                isOccupied = true;
                return player;
            }
            else if (Input.GetKeyDown(interactKey) && doneSpinning)
            {
                interactingPlayer = player;
                playerStates[player] = interactKey;
                return player;
            }
            else if( Input.GetKeyDown(giveUpWeaponKey) && doneSpinning)
            {
                interactingPlayer = player;
                playerStates[player] = giveUpWeaponKey;
                return player;
            }
        }
        return null;
    } //determine if any players are interacting with the interactable
    private void UpdateInteractionStates()
    {
        foreach (Player player in playerStates.Keys)
        {
            switch(player ==  playerWhoSpun)
            {
                case true:
                    //check thru states of box
                    if(doneSpinning)
                        interactable.activeInteraction = interactable.interactions[1]; //prompt to pick up
                    break;
                case false:
                    if (doneSpinning)
                        interactable.activeInteraction = interactable.interactions[3]; //blank
                    break;
            }
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
    private void DeterminePromptOutput(Player.InputState inputState)
    {
        switch (inputState)
        {
            case Player.InputState.KandM:

                break;
            case Player.InputState.Controller:

                break;
        }
    }
    private bool HasEnoughPoints(Player player)
    {
        if (player.points >= costToSpin)
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
        if (playerWhoSpun == null && interactable.interactions[0].player == null)
        {
            Debug.Log("Fart");
            playerWhoSpun = interactable.interactions[0].player;
        }
            

        if (playerWhoSpun != null && HasEnoughPoints(playerWhoSpun) && !isOccupied && !hasSpun)
        {
            Debug.Log(playerWhoSpun);
            InitiateSpin();
            interactable.interactions[0].player = null;
        }
        //reset interacting player
        //interactingPlayer = null;
    }
    private void InitiateSpin()
    {
        DeductPoints(playerWhoSpun);
        PlaySpinAnimation();
        DetermineWeapon();
    }
    private void DeductPoints(Player player)
    {
        player.points -= costToSpin;
    }
    private void DetermineWeapon()
    {
        selectedWeaponPrefab = weaponPrefabs[Random.Range(0, weaponCount)];
        //pickUpWeaponPrompt = "";
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
        weaponUpForGrabs = true;
    } //allow other players to pickup weapon
    public void PickUpWeapon()
    {
        if (interactingPlayer.GetPlayerInventory().doesPlayerHaveAnOpenSlot())
        {
            interactingPlayer.GetPlayerInventory().AddWeapon(selectedWeaponPrefab);
            ResetBoxStatus();
        }
        else
        {
            interactingPlayer.GetPlayerInventory().SwapWeapon(selectedWeaponPrefab);
            ResetBoxStatus();
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
        weaponUpForGrabs = false;
        isOccupied = false;
        selectedWeaponPrefab = null;
    } // for after picking up weapon
}
