using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapKill : MonoBehaviour
{

    [SerializeField] private bool useCollisionEnter;
    [SerializeField] private bool useTriggerEnter;

    private void OnCollisionEnter(Collision collision)
    {
        if (!useCollisionEnter) return;
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.SetDead();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!useTriggerEnter) return;
        if (other.gameObject.GetComponent<PlayerController>())
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.SetDead();
        }
    }
}
