using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawRailTrap : MonoBehaviour
{
    [SerializeField] private GameObject sawPrefab;
    [SerializeField] private GameObject saw;
    [SerializeField] private Collider sawRailCollider;
    [Header("SawRail Positions")]
    [SerializeField] private Transform sawRailLeftLimitTransform;
    [SerializeField] private Transform sawRailRightLimitTransform;
    [SerializeField] private Vector3 offset;
    [SerializeField] private AnimationCurve railCurve;
    [Header("SawRail Speed")]
    [SerializeField] private float sawRailSpeed = 1f;
    [SerializeField] private float sawSpeed = 1000f;
    [Header("Timer")]
    [SerializeField] private float timer;
    [Header("Randomize Time")]
    [SerializeField] private float initiateTime = 2f;
    [SerializeField] private bool randomize = false;
    [SerializeField] private float randomMin = 1f;
    [SerializeField] private float randomMax = 3f;

    private void Awake()
    {
        sawRailCollider = GetComponent<Collider>();

        GameObject sawClone = Instantiate(sawPrefab, transform.position, transform.rotation);
        sawClone.GetComponent<Saw>().sawSpeed = sawSpeed;
        saw = sawClone;

        StartCoroutine(InitiateSawRail());
    }


    private IEnumerator InitiateSawRail()
    {
        if(saw == null) yield return null;

        if (randomize)
            initiateTime = Random.Range(randomMin, randomMax);

        yield return new WaitForSeconds(initiateTime);
        StartCoroutine(SawRailRight());
    }

    private IEnumerator SawRailLeft()
    {
        float cooldown = 0;

        while (cooldown < timer)
        {
            float t = cooldown / timer;

            saw.transform.position = Vector3.Lerp(sawRailRightLimitTransform.position, sawRailLeftLimitTransform.position, railCurve.Evaluate(sawRailSpeed * t));

            cooldown += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(SawRailRight());
    }

    private IEnumerator SawRailRight()
    {
        float cooldown = 0;

        while (cooldown < timer)
        {
            float t = cooldown / timer;

            saw.transform.position = Vector3.Lerp(sawRailLeftLimitTransform.position, sawRailRightLimitTransform.position, railCurve.Evaluate(sawRailSpeed * t));

            cooldown += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(SawRailLeft());
    }

}
