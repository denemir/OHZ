using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovementHandler : MonoBehaviour
{
    public float moveSpeed;
    public float sprintMultiplier = 1.6f;

    private PlayerRotation playerRotation;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playerRotation = GetComponent<PlayerRotation>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(float horizontalInput, float verticalInput, int movementState)
    {
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        float rotationAngle = playerRotation.rotationAngle;

        Vector3 movement = new Vector3();

        switch (movementState)
        {
            case 2:
                movement = moveDirection * (moveSpeed * sprintMultiplier);
                break;
            default:
                movement = moveDirection * moveSpeed;
                break;
        }

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        transform.position = rb.transform.position;
    }
}
