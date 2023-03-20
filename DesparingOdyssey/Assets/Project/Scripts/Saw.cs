using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float sawSpeed = 1000f;

    private void Update()
    {
        transform.Rotate(new Vector3(transform.localRotation.x + 1 * sawSpeed * Time.deltaTime, transform.localRotation.y));
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.SetDead();
        }
    }
}