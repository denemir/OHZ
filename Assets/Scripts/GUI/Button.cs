using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent onClick;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.UI.Button uiButton = GetComponent<UnityEngine.UI.Button>();
        if (uiButton != null)
        {
            uiButton.onClick.AddListener(onMouseClick);
            Debug.Log("UI button acquired");
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void onMouseClick()
    {
        if (onClick != null)
        {
            Debug.Log("Click!");
            onClick.Invoke();
        }
    }
}
