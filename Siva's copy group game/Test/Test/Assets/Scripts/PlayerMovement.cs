using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration = 10f;  // The acceleration factor
    public float deceleration = 10f;  // The deceleration factor
    public float maxSpeed = 5f;      // Max speed (should be pretty obvious)

    private Rigidbody rb;            // Reference to the Rigidbody component
    private Vector3 movementInput;   

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Reading the player's movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        movementInput = new Vector3(horizontalInput, 0f, verticalInput).normalized;
    }

    private void FixedUpdate()
    {
        if (movementInput.magnitude > 0f)
        {
            // Applying acceleration to the player's velocity
            rb.AddForce(movementInput * acceleration, ForceMode.Acceleration);

            // Limiting the player's velocity to the maximum speed
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        else
        {
            // Applying deceleration to slow/stop the player
            Vector3 decelerationForce = -rb.velocity.normalized * deceleration;
            rb.AddForce(decelerationForce, ForceMode.Acceleration);
        }
    }
}
