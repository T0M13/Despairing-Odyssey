using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingBladeTrap : MonoBehaviour
{
    [Header("SwingBlade Positions")]
    [SerializeField] private Quaternion swingBladeLeft;
    [SerializeField] private Quaternion swingBladeRight;
    [SerializeField] private AnimationCurve swingCurve;
    [Header("SwingBlade Speed")]
    [SerializeField] private float swingBladeSpeed;
    [Header("Timer")]
    [SerializeField] private float timer;
    [Header("Randomize Time")]
    [SerializeField] private float initiateTime = 2f;
    [SerializeField] private bool randomize = false;
    [SerializeField] private float randomMin = 1f;
    [SerializeField] private float randomMax = 3f;

    private void Start()
    {
        StartCoroutine(InitiateSwingBlade());
    }

    private IEnumerator InitiateSwingBlade()
    {
        if (randomize)
            initiateTime = Random.Range(randomMin, randomMax);

        yield return new WaitForSeconds(initiateTime);
        StartCoroutine(SwingBladeLeft());
    }

    private IEnumerator SwingBladeLeft()
    {
        float cooldown = 0;

        while (cooldown < timer)
        {
            float t = cooldown / timer;


            transform.rotation = Quaternion.Slerp(swingBladeRight, swingBladeLeft, swingCurve.Evaluate(swingBladeSpeed * t));

            cooldown += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(SwingBladeRight());
    }

    private IEnumerator SwingBladeRight()
    {
        float cooldown = 0;

        while (cooldown < timer)
        {
            float t = cooldown / timer;


            transform.rotation = Quaternion.Slerp(swingBladeLeft, swingBladeRight, swingCurve.Evaluate(swingBladeSpeed * t));

            cooldown += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(SwingBladeLeft());
    }

}
