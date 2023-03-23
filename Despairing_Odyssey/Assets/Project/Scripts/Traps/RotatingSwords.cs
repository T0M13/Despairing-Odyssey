using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSwords : MonoBehaviour
{
    [SerializeField] bool counterclockwise;
    [SerializeField] private int speed;
    [Range(1, 4)]
    [SerializeField] private int swordAmount = 2;
    [SerializeField] private GameObject[] swords = new GameObject[4];

    

    private void Start()
    {
        ChangeDirection();
        SetSwordAmount();  
    }
    private void OnValidate()
    {
        //ChangeDirection();
        SetSwordAmount();
    }

    private void SetSwordAmount()
    {
        for (int i = 0; i < swords.Length; i++)
        {
            swords[i].SetActive(false);
        }

        for (int i = 0; i < swordAmount; i++)
        {
            swords[i].SetActive(true);
        }
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
