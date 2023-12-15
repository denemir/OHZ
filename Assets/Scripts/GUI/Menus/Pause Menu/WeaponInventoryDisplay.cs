using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventoryDisplay : MonoBehaviour
{
    //vars
    public Sprite icon;
    public string text;

    private TextMeshProUGUI textMesh;
    private Image image;

    public Weapon weaponInstance;

    // Start is called before the first frame update
    void Start()
    {
        icon = null;
        text = null;

        if (GetComponentInChildren<TextMeshProUGUI>() != null)
            textMesh = GetComponentInChildren<TextMeshProUGUI>();
        else Debug.Log("TextMeshPro component not found.");

        if (GetComponent<Image>() != null)
            image = GetComponent<Image>();
        else Debug.Log("Image component not found.");

        textMesh.text = "";
        image.color = new Color(0f, 0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(weaponInstance != null)
        {
            text = weaponInstance.weaponName;
            icon = weaponInstance.icon;

            image.sprite = icon;

            textMesh.text = text;
        }
    }

    //updating display
    public void setWeapon(Weapon weapon)
    {
        weaponInstance = weapon;
        text = weaponInstance.weaponName;
        icon = weaponInstance.icon;

        image.sprite = icon;
        image.color = new Color(1f, 1f, 1f, 1f);
        textMesh.text = text;
    }
}
