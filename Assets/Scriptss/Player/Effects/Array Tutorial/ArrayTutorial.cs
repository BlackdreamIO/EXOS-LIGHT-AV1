using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;


public class ArrayTutorial : MonoBehaviour
{
    [SerializeField] float smoothTime;

    [SerializeField] bool startPostProcess;

    [Space]
    [SerializeField] int index;
    public PostEffectScirptableObject[] postEffectScirptables;

    [SerializeField] List<float> list;

    private float currentVelocityVignette, currentVelocityAberration, currentVelocityMotionBlur;
    private float currentVelocitycgSaturation, currentVelocitycgContrast;
    private float currentValueVignette, currentValueAberration, currentValueMotionBlur, currentCgSaturation;

    private float threshold = 0.01f;
    private float vignetteIntencity, chromaticAberrationIntensity, motionBlurIntensity, cgSaturationIntensity;
    private float cgContrastIntensity;

    private PostProcessVolume postProcessVolume;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private ColorGrading colorGrading;

    void Start()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out vignette);
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);
    }

    void Update()
    {
        if (startPostProcess)
        {
            if (index > postEffectScirptables.Length) { return ;}
            
            vignetteIntencity = DampVignette(vignetteIntencity, postEffectScirptables[index].VignetteIntencity);
            chromaticAberrationIntensity = DampChromoticAberration(chromaticAberrationIntensity, postEffectScirptables[index].ChromaticAberrationIntensity);

            vignette.intensity.value = vignetteIntencity;
            chromaticAberration.intensity.value = chromaticAberrationIntensity;

        }
    }

    float DampVignette(float current, float target)
    {
        currentValueVignette = Mathf.SmoothDamp(current, target, ref currentVelocityVignette, smoothTime);
        // Check if the difference between the current value and target value is within the threshold
        if (Mathf.Abs(currentValueVignette - target) <= threshold)
        {
            currentValueVignette = target; // Set the current value to the target value
        }

        return currentValueVignette;
    }
    float DampChromoticAberration(float current, float target)
    {
        currentValueAberration = Mathf.SmoothDamp(current, target, ref currentVelocityAberration, smoothTime);
        
        if (Mathf.Abs(currentValueAberration - target) <= threshold) { currentValueAberration = target; }
        
        return currentValueAberration;
    }
    float DampMotionBlur(float current, float target)
    {
        currentValueMotionBlur = Mathf.SmoothDamp(current, target, ref currentVelocityMotionBlur, smoothTime);
        
        if (Mathf.Abs(currentValueMotionBlur - target) <= threshold) { currentValueAberration = target; }
        
        return currentVelocityMotionBlur;
    }
    float DammColorGradingSaturation(float current, float target)
    {
        currentVelocitycgSaturation = Mathf.SmoothDamp(current, target, ref currentVelocityMotionBlur, smoothTime);

        if (Mathf.Abs(currentCgSaturation - target) <= threshold) { currentCgSaturation = target; }

        return currentCgSaturation;
    }
    float DammColorGradingContrast(float current, float target)
    {
        currentVelocitycgContrast = Mathf.SmoothDamp(current, target, ref currentVelocitycgContrast, smoothTime);

        if (Mathf.Abs(cgContrastIntensity - target) <= threshold) { cgContrastIntensity = target; }

        return currentVelocitycgContrast;
    }
    

}
