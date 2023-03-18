using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTrap : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject trapGrate;
    [SerializeField] private GameObject trapSpikes;
    [Header("Spike Positions")]
    [SerializeField] private Vector3 spikesHidden;
    [SerializeField] private Vector3 spikesShown;
    [Header("Spikes Speed")]
    [SerializeField] private float spikesShootSpeed;
    [SerializeField] private float spikesRetractSpeed;
    [Header("Time till Switch")]
    [SerializeField] private float timer;
    [Header("Randomize Time")]
    [SerializeField] private float initiateTime = 2f;
    [SerializeField] private bool randomize = false;
    [SerializeField] private float randomMin = 1f;
    [SerializeField] private float randomMax = 3f;

    private void Start()
    {
        StartCoroutine(InitiateSpikes());
    }

    private IEnumerator InitiateSpikes()
    {
        if (randomize)
            initiateTime = Random.Range(randomMin, randomMax);

        yield return new WaitForSeconds(initiateTime);
        StartCoroutine(RetractSpikes());
    }

    private IEnumerator ShootSpikes()
    {
        float cooldown = 0;

        while (cooldown < timer)
        {
            float t = cooldown / timer;

            trapSpikes.transform.localPosition = Vector3.Lerp(spikesHidden, spikesShown, spikesShootSpeed * t);

            cooldown += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(RetractSpikes());
    }

    private IEnumerator RetractSpikes()
    {
        float cooldown = 0;

        while (cooldown < timer)
        {
            float t = cooldown / timer;

            trapSpikes.transform.localPosition = Vector3.Lerp(spikesShown, spikesHidden, spikesRetractSpeed * t);

            cooldown += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(ShootSpikes());
    }
}
