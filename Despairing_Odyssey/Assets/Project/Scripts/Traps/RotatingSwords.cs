using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSwords : MonoBehaviour
{
    [SerializeField] bool counterclockwise;
    [SerializeField] private int speed;
    

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
        if (counterclockwise)
            speed = -speed;
        else
            speed = -speed;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
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
