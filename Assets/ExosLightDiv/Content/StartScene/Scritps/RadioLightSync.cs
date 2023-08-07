using UnityEngine;

public class RadioLightSync : MonoBehaviour 
{
    [SerializeField] private Light light;
    [SerializeField] AudioWaveformAnalyzer audioWaveformAnalyzer;

    private void Update() 
    {
        if(light == null) { return; }    

        light.intensity = audioWaveformAnalyzer.GetRealTimeNormalizedAmplitude();

    }

}