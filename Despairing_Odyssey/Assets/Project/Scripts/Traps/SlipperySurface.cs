using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperySurface : MonoBehaviour
{
    public float friction = 0.2f; // The amount of friction to apply to the player's movement

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            // Get the player's rigidbody component
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            // Calculate the direction of movement
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            direction = rb.transform.TransformDirection(direction); // Convert to world space

            // Apply the friction to the movement
            rb.AddForce(-direction.normalized * friction, ForceMode.Impulse);
        }
    }
}
