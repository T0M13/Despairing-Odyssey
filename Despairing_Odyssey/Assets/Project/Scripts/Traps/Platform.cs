using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private bool counterclockwise;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // default axis of rotation
    private PlayerController player;
    private Rigidbody rb;

    private void Start()
    {
        ChangeDirection();
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = rotationAxis * speed * (counterclockwise ? -1f : 1f);
    }

    private void OnValidate()
    {
        ChangeDirection();
    }

    private void ChangeDirection()
    {
        speed = Mathf.Abs(speed) * (counterclockwise ? -1f : 1f);
    }

    private void Update()
    {
        if (player == null) return;
        if (player.IsRagdoll || (player.IsRagdoll && player.IsDead))
        {
            Unparent(player.transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            player = other.gameObject.GetComponent<PlayerController>();
            player.transform.parent = transform;
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
}
