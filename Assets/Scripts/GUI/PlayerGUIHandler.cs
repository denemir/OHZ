using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUIHandler : MonoBehaviour //attaches to player
{
    //player
    public Player currentTargetedPlayer;
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

    public Text currentWaveText;
    public Text currentPointsText;
    public Text ammoInMagText;
    public Text stockAmmoText;
    public Text weaponNameText;

    ////target camera
    //public Camera targetCamera;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayerCanvas();
        playerNameText.text = GetComponent<Player>().playerName;
        //currentTargetedPlayer = GetComponent<Player>();
        //playerCanvas = Instantiate(canvasPrefab, currentTargetedPlayer.transform);

        //currentWaveText = Instantiate(currentWaveTextPrefab, playerCanvas.transform);
        //currentPointsText = Instantiate(currentPointsTextPrefab, playerCanvas.transform);
        //ammoInMagText = Instantiate(ammoInMagTextPrefab, playerCanvas.transform);
        //stockAmmoText = Instantiate(stockAmmoTextPrefab, playerCanvas.transform);
        //weaponNameText = Instantiate(weaponNameTextPrefab, playerCanvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        //    if (currentWaveText != null)
        //    {
        //        currentWaveText.text = currentWave.ToString();
        //        currentWaveText.color = Color.red;
        //    }

        //    if (currentPointsText != null)
        //    {
        //        currentPointsText.text = currentTargetedPlayer.points.ToString();
        //        currentPointsText.color = Color.white;
        //    }

        //    if (ammoInMagText != null)
        //    {
        //        ammoInMagText.text = currentTargetedPlayer.currentWeapon.currentAmmoInMag.ToString();
        //        ammoInMagText.color = Color.blue;
        //    }

        //    if (stockAmmoText != null)
        //    {
        //        stockAmmoText.text = currentTargetedPlayer.currentWeapon.currentStockAmmo.ToString();
        //        stockAmmoText.color = Color.blue;
        //    }

        //    if (weaponNameText != null)
        //    {
        //        weaponNameText.text = currentTargetedPlayer.currentWeapon.weaponName.ToString();
        //        weaponNameText.color = Color.white;
        //    }
    }

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
        if (weaponNameText != null)
        {
            weaponNameText.text = currentTargetedPlayer.currentWeapon.weaponName.ToString();
            weaponNameText.color = Color.white;
        }
    }

    public void UpdateCurrentAmmoInWeapon()
    {
        if (ammoInMagText != null)
        {
            ammoInMagText.text = currentTargetedPlayer.currentWeapon.currentAmmoInMag.ToString();
            ammoInMagText.color = Color.blue;
        }
    }

    public void UpdateCurrentStockAmmo()
    {
        if (stockAmmoText != null)
        {
            stockAmmoText.text = currentTargetedPlayer.currentWeapon.currentStockAmmo.ToString();
            stockAmmoText.color = Color.blue;
            Debug.Log(currentTargetedPlayer.currentWeapon.currentStockAmmo);
        }
    }

    public void UpdateCurrentPoints()
    {
        if (currentPointsText != null)
        {
            currentPointsText.text = currentTargetedPlayer.points.ToString();
            currentPointsText.color = Color.white;
        }
    }

    public void DisplayCurrentPerkIcons()
    {

    }

    public void DisplayCurrentCharacterIcon() //to be used for later
    {

    }
}
