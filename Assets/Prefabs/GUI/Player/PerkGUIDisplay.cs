using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkGUIDisplay : MonoBehaviour
{
    public Perk perk;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if (perk != null)
        {
            image.sprite = perk.icon;
        }
        else image.color = new Color(0f, 0f, 0f, 0f);

    }

    // Update is called once per frame
    void Update()
    {
        if(perk  != null)
        {
            image.sprite = perk.icon;
            image.color = new Color(1f, 1f, 1f, 1f);
        }
        else image.color = new Color(0f, 0f, 0f, 0f);
    }
}
