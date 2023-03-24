using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballBouncer : MonoBehaviour
{

    public float bounceForce = 10f;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            Rigidbody ballRb = other.gameObject.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                Vector3 bounceDirection = (other.transform.position - transform.position).normalized;
                ballRb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
            }
        }
    }
}
