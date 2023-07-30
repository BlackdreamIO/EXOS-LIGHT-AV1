using UnityEngine;

[CreateAssetMenu(fileName = "PostEffect", menuName = "PostLib/ScirptableEffect")]
public class PostEffectScirptableObject : ScriptableObject
{
    [Header("Vignette Setting"), Space(10)]
    public float VignetteIntencity;

    [Header("Chromatic Aberration Setting"), Space(10)]
    public float ChromaticAberrationIntensity;

    [Header("DeptOfField Setting"), Space(10)] 
    public float DeptOfFieldIntensity;

    [Header("LensDetortion Setting"), Space(10)]
    public float LensDetortionIntensity;

}
