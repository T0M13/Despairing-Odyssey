using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapKill : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.SetDead();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.SetDead();
        }
    }
}
