using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RotateingPlatform : MonoBehaviour
{
    [SerializeField] private bool counterclockwise;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // default axis of rotation
    [SerializeField] private Rigidbody platformRigidbody;

    private void Awake()
    {
        platformRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(rotationAxis, speed * Time.fixedDeltaTime);
    }
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
        //if (player == null) return;
        //if (player.IsRagdoll || (player.IsRagdoll && player.IsDead))
        //{
        //    Unparent(player.transform);
        //}
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<PlayerController>())
        {
            //player = col.gameObject.GetComponent<PlayerController>();
            //player.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.GetComponent<PlayerController>())
        {
            Unparent(col.gameObject.transform);
        }
    }

    private void Unparent(Transform transform)
    {
        transform.parent = null;
    }
}