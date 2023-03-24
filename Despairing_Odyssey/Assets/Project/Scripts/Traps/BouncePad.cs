using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceVelocity = 10.0f;
    public Vector3 bounceDirection = Vector3.up;

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<PlayerController>())
        {
            Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 velocity = bounceDirection.normalized * bounceVelocity;
                rb.velocity = velocity;
            }
        }
    }
}
