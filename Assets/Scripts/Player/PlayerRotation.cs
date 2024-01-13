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
    private Vector2 screenPoint = new Vector3(Screen.width / 2f, Screen.height / 2f); //screen point that is used as basis for rotation
    private Vector2 mousePosition;

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
    void LateUpdate()
    {
        if(player.inputState != inputState)
        {
            inputState = player.inputState;
        }


        switch (inputState)
        {
            case Player.InputState.KandM:
                screenPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
                mousePosition = Input.mousePosition;
                rotationAngle = Mathf.Atan2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y) * Mathf.Rad2Deg; //idk what it is about this code but for some reason when the player spins rapdily, it causes the frame rate to drop basically in half. no clue why.
                break;
            case Player.InputState.Controller:
                rightThumbStickDirection = new Vector3(Input.GetAxis("Horizontal Aim"), 0f, Input.GetAxis("Vertical Aim"));
                if (rightThumbStickDirection.magnitude > 0.3f)
                {
                    Vector3 inputDirection = new Vector3(rightThumbStickDirection.x, 0f, rightThumbStickDirection.z).normalized;
                    float targetRotationAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                    rotationAngle = targetRotationAngle + 90;
                }
                break;
        }

        GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0f, rotationAngle, 0f)); //this was in place of raw transform! don't use raw transform, rather use rigid body MoveRotation to prevent full path rotations


    }

    //private void LateUpdate()
    //{

    //}
}
