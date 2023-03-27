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
    [Range(0, 1)]
    [SerializeField] private float lensDistortionLerpTime;
    [SerializeField] private LensDistortion lensDistortion = null;
    [SerializeField] private float lensIntensityMax = 1f;
    [SerializeField] private float lensIntensityMin = 0f;


    void Start()
    {
        if (profile == null) return;
        GetLensDistortion();

       StartCoroutine( ChangeLensDistortion());
    }

    private void GetLensDistortion()
    {
        profile.TryGet(out lensDistortion);
    }

    private IEnumerator ChangeLensDistortion()
    {
        if (profile == null) yield break;

        yield return null;

        while (lensDistortionLerpTime >= 1)
        {

            if (lensDistortion != null)
            {
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, lensIntensityMax, Time.deltaTime * lensDistortionLerpTime);
            }
            else if (lensDistortion != null)
            {
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, lensIntensityMin, Time.deltaTime * lensDistortionLerpTime);
            }
        }
    }
}
