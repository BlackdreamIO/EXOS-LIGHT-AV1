using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;

public class PostEngine : MonoBehaviour
{
    [Header("POST ENGINE")]
    public PostEffectScirptableObject[] postEffectScirptable;
    public int index;
    [SerializeField] float smoothTime;
    [SerializeField] float currentValueVignette;
    [SerializeField] float currentValueChromoticAberration;
    [SerializeField] float currentValuesLensDistortion;

    #region Scirpt Variables
    private float currentVelocity;
    float[] currentValues;
    private float threshold = 0.01f;
    [HideInInspector][SerializeField] List<float> targetValues;
    [HideInInspector][SerializeField] float[] intensities;
    private PostProcessVolume postProcessVolume;
    private Vignette vignetteComponent;
    private ChromaticAberration aberrationComponent;
    private LensDistortion lensDistortionComponent;

    #endregion

    void Start()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();

        postProcessVolume.sharedProfile.TryGetSettings(out vignetteComponent);
        postProcessVolume.sharedProfile.TryGetSettings(out aberrationComponent);
        postProcessVolume.sharedProfile.TryGetSettings(out lensDistortionComponent);

        InitializeScriptableObject();
    }

    void InitializeScriptableObject()
    {
        targetValues = new List<float>();
        targetValues.Add(postEffectScirptable[index].VignetteIntencity);
        targetValues.Add(postEffectScirptable[index].ChromaticAberrationIntensity);
        targetValues.Add(postEffectScirptable[index].LensDetortionIntensity);

        intensities = new float[targetValues.Count];
        currentValues = new float[targetValues.Count];

        // Set the initial values of intensities and currentValuesVignette based on the targetValues
        for (int i = 0; i < targetValues.Count; i++)
        {
            intensities[i] = targetValues[i];
            currentValues[i] = targetValues[i];
        }
    }

    void Update()
    {
        UpdateScriptableObjectValues();
        UpdateIntensities();
        UpdateCurrentValue();
        UpdatePostEffect();
    }

    void UpdatePostEffect()
    {
        vignetteComponent.intensity.value = currentValueVignette;
        aberrationComponent.intensity.value = currentValueChromoticAberration;
    }

    void UpdateScriptableObjectValues()
    {
        targetValues[0] = postEffectScirptable[index].VignetteIntencity;
        targetValues[1] = postEffectScirptable[index].ChromaticAberrationIntensity;
        targetValues[2] = postEffectScirptable[index].LensDetortionIntensity;
    }
    void UpdateCurrentValue()
    {
        currentValueVignette = intensities[0];
        currentValueChromoticAberration = intensities[1];
        currentValuesLensDistortion = intensities[2];
    }
    void UpdateIntensities()
    {
        for (int i = 0; i < targetValues.Count; i++)
        {
            intensities[i] = DampValues(intensities[i], targetValues[i]);
        }
    }
    float DampValues(float current, float target)
    {
        float currentValue = Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime);

        if (Mathf.Abs(currentValue - target) <= threshold)
        {
            currentValue = target;
        }

        return currentValue;
    }
    
}
