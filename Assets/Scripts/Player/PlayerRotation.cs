using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRotation : MonoBehaviour
{
    //controller
    private Vector3 rightThumbStickDirection; //set for controllers
    //public float rightThumbStickSensitivity;

    //k&m
    private Vector3 screenPoint = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f); //screen point that is used as basis for rotation
    private Vector3 mousePosition;
    private Vector3 mouseDirection;

    //player
    private Player player;
    private Player.InputState inputState;

    public float rotationAngle { get; private set; }

    void Start()
    {
        if (GetComponent<Player>() != null)
            player = GetComponent<Player>();
        else Debug.Log("Player component missing.");
        inputState = player.inputState;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.inputState != inputState)
        {
            inputState = player.inputState;
        }
        

        switch (inputState)
        {
            case Player.InputState.KandM:
                screenPoint = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
                mousePosition = Input.mousePosition;
                mouseDirection = mousePosition - screenPoint;
                rotationAngle = Mathf.Atan2(mouseDirection.x, mouseDirection.y) * Mathf.Rad2Deg;
                //Debug.Log("Rotate angle: " + rotationAngle);
                break;
            case Player.InputState.Controller:
                rightThumbStickDirection = new Vector3(Input.GetAxis("Horizontal Aim"), 0f, Input.GetAxis("Vertical Aim"));
                if (rightThumbStickDirection.magnitude > 0.1f)
                {
                    Vector3 inputDirection = new Vector3(rightThumbStickDirection.x, 0f, rightThumbStickDirection.z).normalized;
                    float targetRotationAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                    rotationAngle = targetRotationAngle + 90;
                }
                break;
        }

        transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);


    }
}
