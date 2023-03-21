using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RotateingPlatform : MonoBehaviour
{
    [SerializeField] bool counterclockwise;
    [SerializeField] private int speed;

    private void Start()
    {
        if (counterclockwise)
            speed = -speed;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }

    private void OnTriggerStay(Collider col)
    {
        col.transform.parent = transform;
    }

    private void OnTriggerExit(Collider col)
    {
        col.transform.parent = null;
    }
}
