using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBarrel : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 axis = Vector3.up;
    public bool rotateClockwise = true; 

    void Update()
    {
        float direction = rotateClockwise ? 1f : -1f;

        transform.Rotate(axis, direction * speed * Time.deltaTime);
    }
}
