using UnityEngine;
using System.Collections.Generic;

public class MultiDamp : MonoBehaviour
{
    public PostEffectScirptableObject[] postEffectScirptable;
    [SerializeField] int index;
    [SerializeField] float smoothTime;
    [SerializeField] List<float> targetValues;
    [SerializeField] float[] intensities;
    private float currentVelocityV;
    private float[] currentValues;
    private float threshold = 0.01f;

    void Start()
    {
        InitializeScriptableObject();
    }

    void InitializeScriptableObject()
    {
        targetValues = new List<float>();

        targetValues.Add(postEffectScirptable[index].VignetteIntencity);
        targetValues.Add(postEffectScirptable[index].ChromaticAberrationIntensity);
        targetValues.Add(postEffectScirptable[index].DeptOfFieldIntensity);

        intensities = new float[targetValues.Count];
        currentValues = new float[targetValues.Count];

        // Set the initial values of intensities and currentValuesVignette based on the targetValues
        for (int i = 0; i < targetValues.Count; i++)
        {
            currentValues[i] = targetValues[i];
        }
    }

    void Update()
    {
        UpdateScriptableObjectValues();
        UpdateIntensities();
    }

    void UpdateScriptableObjectValues()
    {
        if(index <= postEffectScirptable.Length)
        {
            targetValues[0] = postEffectScirptable[index].VignetteIntencity;
            targetValues[1] = postEffectScirptable[index].ChromaticAberrationIntensity;
            targetValues[2] = postEffectScirptable[index].DeptOfFieldIntensity;
        }
    }

    void UpdateIntensities()
    {
        for (int i = 0; i < targetValues.Count; i++)
        {
            intensities[i] = DampPostEffect(intensities[i], targetValues[i]);
        }
    }

    float DampPostEffect(float current, float target)
    {
        float currentValue = Mathf.SmoothDamp(current, target, ref currentVelocityV, smoothTime);

        if (Mathf.Abs(currentValue - target) <= threshold)
        {
            currentValue = target;
        }

        return currentValue;
    }
}
