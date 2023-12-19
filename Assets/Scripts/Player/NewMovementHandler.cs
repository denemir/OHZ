using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class NewMovementHandler : MonoBehaviour
{
    //vals
    public float moveSpeed;
    public float sprintMultiplier = 1.6f;
    public float gravityScale;
    public float jumpForce;

    private PlayerRotation playerRotation;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playerRotation = GetComponent<PlayerRotation>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        ApplyCustomGravity();
    }

    //movement & gravity (fun stuff here)
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
    public void Jump()
    {
        Vector3 direction = Vector3.up;

        //applying force to player
        rb.AddForce(direction * jumpForce, ForceMode.Force);
        Debug.Log("Jump!");
    }

    private void ApplyCustomGravity()
    {
        //direction & force
        Vector3 gravityDirection = Vector3.down;
        Vector3 gravityForce = gravityDirection * Mathf.Abs(Physics.gravity.magnitude) * gravityScale;

        //applying
        rb.AddForce(gravityForce, ForceMode.Force);
    }

    //checks
    private void isGrounded()
    {

    } //determine if player is currently standing on something
}
