using UnityEngine;

[CreateAssetMenu(fileName = "BobConfigure", menuName = "Motion/HeadBOB/HeadBobConfigure")]
public class HeadBOBConfigure : ScriptableObject 
{
    public bool _enableHeadBob = true;
    public float _frequency = 15;
    public float _amplitude = 2;
    public float _smooth = 0.3f;

}