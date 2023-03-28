using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballBouncer : MonoBehaviour
{
    public float pushForce = 10f; // The amount of force to push the player away
    public float pushRadius = 1f; // The radius of the push force
    public LayerMask Player; // The layer the player is on

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Player)
        {
            // Calculate the direction to push the player away from the center of the bouncer
            Vector3 pushDirection = other.transform.position - transform.position;
            pushDirection = pushDirection.normalized;

            // Apply the push force to the player
            other.attachedRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the push radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pushRadius);
    }
}