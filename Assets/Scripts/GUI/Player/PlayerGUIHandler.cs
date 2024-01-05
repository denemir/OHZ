using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerGUIHandler : MonoBehaviour //attaches to player
{
    //player
    public Player currentTargetedPlayer;
    private PlayerInventory inventory;
    public Canvas playerCanvas;
    public Canvas canvasPrefab;

    //match
    public int currentWave = 1;
    // implement other players to display

    //texts
    public Text playerNameText;
    public Text currentWaveTextPrefab;
    public Text currentPointsTextPrefab;
    public Text ammoInMagTextPrefab;
    public Text stockAmmoTextPrefab;
    public Text weaponNameTextPrefab;
    public Text interactionPromptTextPrefab;

    public Text currentWaveText;
    public Text currentPointsText;
    public Text ammoInMagText;
    public Text stockAmmoText;
    public Text weaponNameText;
    public Text interactPromptText;

    ////target camera
    //public Camera targetCamera;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayerCanvas();

        currentTargetedPlayer = GetComponent<Player>();
        inventory = GetComponent<PlayerInventory>();

        currentWaveText = Instantiate(currentWaveTextPrefab, playerCanvas.transform);
        currentPointsText = Instantiate(currentPointsTextPrefab, playerCanvas.transform);
        ammoInMagText = Instantiate(ammoInMagTextPrefab, playerCanvas.transform);
        stockAmmoText = Instantiate(stockAmmoTextPrefab, playerCanvas.transform);
        weaponNameText = Instantiate(weaponNameTextPrefab, playerCanvas.transform);
        interactPromptText = Instantiate(interactionPromptTextPrefab, playerCanvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (doesPlayerComponentExist())
        {
            UpdatePlayerGUI();
        }
    }

    //initialization
    public void CreatePlayerCanvas()
    {
        if (canvasPrefab != null)
            playerCanvas = Instantiate(canvasPrefab);
        else Debug.LogWarning("Canvas not found");
        playerCanvas.worldCamera = GetComponent<Player>().activeCamera;

        //set canvas position based on player id
        int playerID = GetComponent<Player>().id;

        float canvasWidth = 0.5f;
        float canvasHeight = 0.5f;

        float xOffset = (playerID % 2) * canvasWidth;
        float yOffset = (playerID / 2) * canvasHeight;

        playerCanvas.transform.SetParent(null);
        playerCanvas.transform.position = new Vector3(xOffset, yOffset, 0);
        playerCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasWidth, canvasHeight);
 
    }

    //updates to gui elements
    public void UpdatePlayerGUI()
    {
        UpdateCurrentWeapon();
        UpdateCurrentAmmoInWeapon();
        UpdateCurrentStockAmmo();
    }
    public void UpdateCurrentWave()
    {
        currentWave++;
        if (currentWaveText != null)
        {
            currentWaveText.text = currentWave.ToString();
            currentWaveText.color = Color.red;
        }
    }
    public void UpdateCurrentWeapon()
    {
        if (inventory.GetCurrentWeapon() != null)
        {
            weaponNameText.text = inventory.GetCurrentWeapon().name.ToString();
        }
    }
    public void UpdateCurrentAmmoInWeapon()
    {
        if (inventory.GetCurrentWeapon() != null)
        {
            ammoInMagText.text = inventory.GetCurrentWeapon().currentAmmoInMag.ToString();
            if(inventory.GetCurrentWeapon().currentAmmoInMag <= (inventory.GetCurrentWeapon().magazineSize * 0.25)) //if weapon is less than or equal to 25% of mag size, indicate low ammo
                ammoInMagText.color = Color.red;
            else ammoInMagText.color = Color.white;
        }
    }
    public void UpdateCurrentStockAmmo()
    {
        if (doesPlayerHaveWeapon())
        {
            stockAmmoText.text = currentTargetedPlayer.GetCurrentWeapon().currentStockAmmo.ToString();
            if (currentTargetedPlayer.GetCurrentWeapon().currentStockAmmo < (inventory.GetCurrentWeapon().magazineSize)) //if stock ammo is less than a mag size, indiciate low overall ammo
                stockAmmoText.color = Color.red;
            else stockAmmoText.color = Color.white;            
        }
    }
    public void UpdateCurrentPoints()
    {
        if (currentPointsText != null)
        {
            currentPointsText.text = currentTargetedPlayer.playerStats.points.ToString();
            currentPointsText.color = Color.white;
        }
    }

    //display
    public void DisplayCurrentPerkIcons()
    {

    }
    public void DisplayCurrentCharacterIcon() //to be used for later
    {

    }
    public void DisplayEventPrompt(Interactable.Interaction interaction)
        {
            interactPromptText.text = interaction.prompt;
        }

    public void RemovePrompt()
    {
        interactPromptText.text = "";
    }

    //checks
    private bool doesPlayerHaveWeapon()
    {
        return currentTargetedPlayer.GetCurrentWeapon() != null;
    }

    private bool doesPlayerComponentExist()
    {
        return GetComponent<Player>() != null;
    }
}
