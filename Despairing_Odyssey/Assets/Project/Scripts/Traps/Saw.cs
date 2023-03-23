using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float sawSpeed = 1000f;

    private void Start()
    {
        if (AudioManager.instance)
            AudioManager.instance.PlayOnObject("saw", gameObject);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(transform.localRotation.x + 1 * sawSpeed * Time.deltaTime, transform.localRotation.y, 0));
    }


}
