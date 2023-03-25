using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceVelocity = 10.0f;
    public float bounceThreshold = -5f;
    public Vector3 bounceDirection = Vector3.up;

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<PlayerController>() && col.GetComponent<Rigidbody>().velocity.y <= bounceThreshold)
        {
            PlayerController player = col.gameObject.GetComponent<PlayerController>();

            Rigidbody rb = player.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 velocity = bounceDirection.normalized * bounceVelocity;
                rb.velocity = velocity;
                player.JumpBehaviour.PlayJumpAnimation(player.PlayerAnim);
            }
        }
    }
}
