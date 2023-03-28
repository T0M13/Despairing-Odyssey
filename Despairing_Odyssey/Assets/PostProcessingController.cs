using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Events;

public class PostProcessingController : MonoBehaviour
{
    [Header("Post Process")]
    [SerializeField] private VolumeProfile profile;
    [Header("Lens Distortion")]
    [SerializeField] private float lensDistortionLerpTime = 1f;
    [SerializeField] private LensDistortion lensDistortion = null;
    [SerializeField] private float lensIntensityMax = 1f;
    [SerializeField] private float lensIntensityMin = 0f;
    [SerializeField] private bool lensIntensityLerpIn = false;
    [SerializeField] private bool lensToggler = false;

    [SerializeField] private float lensMultiplierMax = 1f;
    [SerializeField] private float lensMultiplierMin = 0.5f;
    [SerializeField] private float lensScaleMax = 1f;
    [SerializeField] private float lensScaleMin = 0.01f;



    void Start()
    {
        if (profile == null) return;
        GetLensDistortion();

        StartCoroutine(ChangeLensDistortion(lensIntensityLerpIn));
    }

    private void GetLensDistortion()
    {
        profile.TryGet(out lensDistortion);
    }

    private void OnValidate()
    {
        if (lensToggler)
        {
            lensToggler = false;
            StartCoroutine(ChangeLensDistortion(lensIntensityLerpIn));
        }
    }

    private IEnumerator ChangeLensDistortion(bool lerpIn)
    {
        if (profile == null) yield break;

        if (lerpIn)
        {
            lensDistortion.intensity.value = lensIntensityMax;
            lensDistortion.xMultiplier.value = lensMultiplierMin;
            lensDistortion.yMultiplier.value = lensMultiplierMin;
            lensDistortion.scale.value = lensScaleMin;
        }
        else
        {
            lensDistortion.intensity.value = lensIntensityMin;
            lensDistortion.xMultiplier.value = lensMultiplierMax;
            lensDistortion.yMultiplier.value = lensMultiplierMax;
            lensDistortion.scale.value = lensScaleMax;
        }

        yield return new WaitForSeconds(1f);

        float cooldown = 0;

        while (cooldown < lensDistortionLerpTime)
        {
            float t = cooldown / lensDistortionLerpTime;

            if (lerpIn)
            {
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, lensIntensityMin, 1 * t);
                lensDistortion.xMultiplier.value = Mathf.Lerp(lensDistortion.xMultiplier.value, lensMultiplierMax, 1 * t);
                lensDistortion.yMultiplier.value = Mathf.Lerp(lensDistortion.yMultiplier.value, lensIntensityMax, 1 * t);
                lensDistortion.scale.value = Mathf.Lerp(lensDistortion.scale.value, lensScaleMax, 1 * t);

            }
            else
            {
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, lensIntensityMax, 1 * t);
                lensDistortion.xMultiplier.value = Mathf.Lerp(lensDistortion.xMultiplier.value, lensMultiplierMin, 1 * t);
                lensDistortion.yMultiplier.value = Mathf.Lerp(lensDistortion.yMultiplier.value, lensMultiplierMin, 1 * t);
                lensDistortion.scale.value = Mathf.Lerp(lensDistortion.scale.value, lensScaleMin, 1 * t);
            }

            cooldown += Time.deltaTime;
            yield return null;
        }

    }
}
