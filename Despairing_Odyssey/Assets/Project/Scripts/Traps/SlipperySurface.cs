using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperySurface : MonoBehaviour
{
    public float slideSpeed = 5f; // speed at which player slides on surface
    public float slideAngle = 45f; // maximum angle at which player can control movement on surface
    public float slideSmoothness = 0.5f; // amount of smoothing applied to sliding velocity

    private bool isSliding = false;
    private Vector3 slideDirection;
    private Rigidbody playerRB;
    private Vector3 slideVelocity = Vector3.zero;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            float angle = Vector3.Angle(collision.contacts[0].normal, Vector3.up);
            if (angle > slideAngle)
            {
                isSliding = true;
                slideDirection = Vector3.ProjectOnPlane(collision.gameObject.transform.forward, collision.contacts[0].normal).normalized;
                playerRB = collision.gameObject.GetComponent<Rigidbody>();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            // Apply sliding velocity to player
            if (isSliding)
            {
                Vector3 targetVelocity = slideDirection * slideSpeed;
                slideVelocity = Vector3.Lerp(slideVelocity, targetVelocity, slideSmoothness);
                playerRB.velocity = slideVelocity;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            isSliding = false;
        }
    }
}
