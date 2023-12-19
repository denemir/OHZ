using System.Collections;
using System.Collections.Generic;
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
    //and all cost 10 points.
    public enum FireSaleState
    {
        Active,
        Inactive
    }
    public FireSaleState fireSaleState;

    //stats
    public int costToSpin;

    public KeyCode interactKey;
    public KeyCode giveUpWeaponKey; //key specifically for allowing other players to pick up spun weapon

    //available weapons
    public List<Weapon> weapons;
    public int weaponCount;
    private double weaponOdds; //determined by 1/weaponCount

    //interaction
    private Interactable interactable;
    private bool isInitialized = false;

    public UnityEvent spinBox;
    public UnityEvent pickUpWeapon; //for player who bought the mystery box
    public string pickUpWeaponPrompt;
    public UnityEvent acceptWeapon; //if the player who purchased the mystery box chose to give up their weapon

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
        if (isInitialized)
        {
            switch (fireSaleState)
            {
                case FireSaleState.Active:
                    costToSpin = 10;
                    interactable.interactions[0].prompt = "Hold F to spin the mystery box for " + costToSpin + " points";
                    break;
                case FireSaleState.Inactive:
                    costToSpin = 950;
                    interactable.interactions[0].prompt = "Hold F to spin the mystery box for " + costToSpin + " points";
                    break;
            }
        }
        else InitializeInteractions();


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
            prompt = "Hold F to pickup weapon or Hold E to let other players take it",
            key = interactKey,
            action = pickUpWeapon
        });

        //let other players take weapon
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold F to pickup weapon",
            key = giveUpWeaponKey,
            action = acceptWeapon
        });

        interactable.activeInteraction = interactable.interactions[0];
    }

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

    private void HasEnoughPoints()
    {

    }

    //spin
    private void DeductPoints()
    {

    }
    private void DetermineWeapon()
    {

        //pickUpWeaponPrompt = "";
    }
    private void PlaySpinAnimation() //cycles thru all possible weapons from mystery box
    {
        //needs a cycling weapon object
    }
    private void PlayMysteryBoxJingle()
    {

    }
    private void PlaySpecialEffect()
    {

    }


    //post-spin
    private void GiveUpWeapon()
    {

    } //allow other players to pickup weapon
    private void PickUpWeapon()
    {

    }
    private void CloseLid()
    {

    } //closes the mystery box
}
