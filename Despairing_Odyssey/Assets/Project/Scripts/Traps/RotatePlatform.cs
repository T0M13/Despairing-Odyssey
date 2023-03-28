using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RotatePlatform : MonoBehaviour
{
    public float rotationSpeed = 10f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(Vector3.up * rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
        rb.angularVelocity = Vector3.up * rotationSpeed;
    }

   
}
