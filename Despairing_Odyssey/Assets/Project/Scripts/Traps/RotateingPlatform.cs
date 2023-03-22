using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RotateingPlatform : MonoBehaviour
{
    [SerializeField] bool counterclockwise;
    [SerializeField] private int speed;
    private PlayerController player;

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
        if (player == null) return;
        if (player.IsRagdoll || (player.IsRagdoll && player.IsDead))
        {
            Unparent(player.transform);
        }
        
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.GetComponent<PlayerController>())
        {
            player = col.GetComponent<PlayerController>();


            player.transform.parent = transform;
        }

    }

    private void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<PlayerController>())
        {
            Unparent(col.transform);
        }
    }

    private void Unparent(Transform transform)
    {
        transform.parent = null;
    }
}
