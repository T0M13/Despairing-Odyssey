using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForceMagnitude = 10.0f; 
    public Vector3 bounceForceDirection = Vector3.up; 

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<PlayerController>())
        {
            Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 bounceVector = bounceForceMagnitude * bounceForceDirection.normalized;
                rb.AddForce(bounceVector, ForceMode.Impulse);
            }
        }
    }
}
