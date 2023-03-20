using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPlatform : MonoBehaviour
{
    [SerializeField] private float disappearTime = 2f;
    [SerializeField] private float reappearTime = 5f;
    private Collider platformcollider;
    private MeshRenderer platformmesh;
    [SerializeField] private bool isDisappeard;
    private void Awake()
    {
        platformcollider = gameObject.GetComponent<Collider>();
        platformmesh = gameObject.GetComponent<MeshRenderer>();
    }
    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<PlayerController>() && !isDisappeard)
        {
            isDisappeard = true;
            StartCoroutine(Disappear());
        }
    }
    IEnumerator Reappear()
    {
        yield return new WaitForSeconds(reappearTime);
        platformcollider.enabled = true;
        platformmesh.enabled = true;
        isDisappeard = false;
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearTime);
        platformcollider.enabled = false;
        platformmesh.enabled = false;
        StartCoroutine(Reappear());
    }

}
