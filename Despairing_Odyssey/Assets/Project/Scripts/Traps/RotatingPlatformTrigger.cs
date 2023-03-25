using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatformTrigger : MonoBehaviour
{
    [SerializeField] private bool counterclockwise;
    [SerializeField] private int speed;
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // default axis of rotation
    private PlayerController player;

    private bool hasCollidedWithNormalCollider = false;
    private bool hasCollidedWithTriggerCollider = false;

    private void Start()
    {
        ChangeDirection();
    }

    private void OnValidate()
    {
        ChangeDirection();
    }

    private void ChangeDirection()
    {
        speed = counterclockwise ? -Mathf.Abs(speed) : Mathf.Abs(speed);
    }

    private void Update()
    {
        transform.Rotate(rotationAxis, speed * Time.deltaTime);
        if (player == null) return;
        if (player.IsRagdoll || (player.IsRagdoll && player.IsDead))
        {
            Unparent(player.transform);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())

        {
            hasCollidedWithNormalCollider = true;
            CheckCollision();
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.GetComponent<PlayerController>())
        {
            hasCollidedWithTriggerCollider = true;
            CheckCollision();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            Unparent(other.gameObject.transform);
        }
    }

    private void Unparent(Transform transform)
    {
        transform.parent = null;
    }


    private void CheckCollision()
    {
        if (hasCollidedWithNormalCollider && hasCollidedWithTriggerCollider)
        {
            if (gameObject.GetComponent<PlayerController>())
            {
                player = gameObject.GetComponent<PlayerController>();
                player.transform.parent = transform;
            }
        }
    }

}
