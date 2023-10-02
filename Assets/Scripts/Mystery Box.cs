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

    public string spinPrompt;
    public UnityEvent spinBox;


    public string pickUpWeaponPrompt;
    public UnityEvent pickUpWeapon;

    public string giveUpWeaponPrompt;
    public UnityEvent giveUpWeapon;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Interactable>() != null)
        {
            interactable = GetComponent<Interactable>();
        }
        else Debug.Log("Interactable component not found on Mysterybox. Please attach Interactable script to Mysterybox Prefab.");

        //purchase mystery box
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = spinPrompt,
            key = interactKey,
            action = spinBox
        }); //Mysterybox Spin prompt

        //pick up weapon
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = pickUpWeaponPrompt,
            key = interactKey,
            action = pickUpWeapon
        });

        //let other players take weapon
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = giveUpWeaponPrompt,
            key = giveUpWeaponKey,
            action = giveUpWeapon
        });
    }

    // Update is called once per frame
    void Update()
    {
        switch (fireSaleState)
        {
            case FireSaleState.Active:
                costToSpin = 10;
                break;
                case FireSaleState.Inactive:
                costToSpin = 950;
                break;
        }

    }

    //pre-spin
    private void DeterminePromptOutput(Player.InputState inputState)
    {
        switch(inputState)
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

        pickUpWeaponPrompt = "";
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
