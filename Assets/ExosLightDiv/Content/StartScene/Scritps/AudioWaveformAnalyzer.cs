using UnityEngine;
public class AudioWaveformAnalyzer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float analysisTime = 0.1f; // Time window for waveform analysis in seconds
    [SerializeField] private float maxAmplitude = 0.5f; // Adjust this value based on your audio clip characteristics
    [SerializeField] private float output, outputTwo, scale = 5f;
    // Update is called once per frame
    private void Update()
    {
        float currentTime = Time.time; // Current time in seconds
        float amplitude = GetWaveformAmplitude(currentTime);
        // Do something with the amplitude value, like triggering an effect or action
        output = amplitude;
        outputTwo = GetRealTimeNormalizedAmplitude();
    }

    // Function to calculate normalized amplitude of waveform at the current audio time
    public float GetRealTimeNormalizedAmplitude()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            float time = audioSource.time; // Current playback time
            float amplitude = GetWaveformAmplitude(time);
            float normalizedAmplitude = Mathf.Clamp01(amplitude / maxAmplitude); // Normalize between 0 and 1
            return normalizedAmplitude * scale; // Scale to the range of 0 to 5
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing.");
            return 0f;
        }
    }

    public float GetWaveformAmplitude(float time)
    {
        if (audioSource != null && audioSource.clip != null)
        {
            int sampleRate = audioSource.clip.frequency;
            int sampleStart = Mathf.FloorToInt(time * sampleRate);
            int samplesToAnalyze = Mathf.FloorToInt(analysisTime * sampleRate);

            float[] samples = new float[samplesToAnalyze];
            audioSource.clip.GetData(samples, sampleStart);

            float sum = 0f;
            for (int i = 0; i < samplesToAnalyze; i++)
            {
                sum += Mathf.Abs(samples[i]);
            }

            float averageAmplitude = sum / samplesToAnalyze;
            return averageAmplitude;
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing.");
            return 0f;
        }
    }

    // Public getter method for the output value
    public float GetOutputValue()
    {
        return output;
    }
}
