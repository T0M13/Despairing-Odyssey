using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    //[SerializeField] private float damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.SetDead();
        }
    }

}
