using UnityEngine;
using static Player;

public class MovementHandler : MonoBehaviour
{
    //private float horizontalInput;
    //private float verticalInput;

    private Vector3 moveVector; //current direction player is moving in
    private Vector2 smoothVelocity;

    private float smoothInputSpeed = 0f;

    public void Move(float horizontalInput, float verticalInput, float moveSpeed, int movementState /*Determines whether player is sprinting or not*/)
    {
        Vector2 input = new Vector2(horizontalInput, verticalInput);
        moveVector = Vector2.SmoothDamp(moveVector, input, ref smoothVelocity, smoothInputSpeed);
        moveVector = new Vector3(moveVector.x, 0f, moveVector.y);

        switch (movementState)
        {
            case 1: //Walking
                transform.position += moveSpeed * Time.deltaTime * moveVector;
                break;
            case 2: //Sprinting
                transform.position += (moveSpeed * 1.6f) * Time.deltaTime * moveVector;
                break;
        }
    }
}
