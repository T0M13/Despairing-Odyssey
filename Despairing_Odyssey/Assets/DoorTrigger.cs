using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private bool useCollisionEnter;
    [SerializeField] private bool useTriggerEnter;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject button;
    private bool doorOpen = false;
    
    

    private void OnCollisionEnter(Collision collision)
    {
        if (!useCollisionEnter) return;
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            if (!doorOpen)
            {
                door.GetComponent<Animation>().Play("Open_Slidedoor_Anim");
                button.GetComponent<Animation>().Play("Button_Down");
                doorOpen = true;

            }

            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!useTriggerEnter) return;
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (!doorOpen)
            {
                door.GetComponent<Animation>().Play("Open_Slidedoor_Anim");
                button.GetComponent<Animation>().Play("Button_Down");
                doorOpen = true;

            }
        }
    }
}
