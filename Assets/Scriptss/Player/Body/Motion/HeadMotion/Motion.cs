using UnityEngine;
using TetraCreations.Attributes;
using EL.Core.Player;
public class Motion : MonoBehaviour
{   
    [Title("WALK MOTION", TitleColor.Aqua, TitleColor.Fuchsia, 2,20)]
    [SerializeField] private float m_Intensity = 5f;
    [SerializeField] private float m_Amount = 1f;
    [SerializeField] private bool m_InvertX, m_InvertY;

    [Title("RUN MOTION", TitleColor.Aqua, TitleColor.Orange, 2, 20)]
    [SerializeField] float speed = 1.0f;
    [SerializeField] float amplitude = 1.0f;
    [SerializeField] float offset = 0.0f;

    [SerializeField] GameObject targetObject;
    [SerializeField] GameObject camHolder;

    [SerializeField] Player playerScript;

    bool _enableWalkMotion = true;

    private void Update()
    {
        if(_enableWalkMotion) 
        { 
            WalkMotion();
            CheckForRunTrigger(); 
        }
    }
    private void WalkMotion()
    {
        //, Get InputAxis
        float movementX = Input.GetAxis("Horizontal") * m_Intensity;
        float movementY = Input.GetAxis("Vertical") * m_Intensity;

        //, Toggle Positive Negetive Value From Boolean
        float outputY = m_InvertY ? -movementY : movementY;
        float outputX = m_InvertX ? -movementX : movementX;

        //, Handle The Rotation Of Target Object
        Quaternion a = Quaternion.identity;
        Quaternion b = Quaternion.Euler(outputY, 0, outputX);
        targetObject.transform.localRotation = Quaternion.Slerp(a, b, m_Amount);
    }


    private void CheckForRunTrigger()
    {
        float time = Time.time * speed;
        float sineValue = Mathf.Sin(time) * amplitude + offset;

        if (playerScript.playerState != Player.PlayerState.running)
        {
            camHolder.transform.localRotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        float newPosition = camHolder.transform.localRotation.z;
        newPosition = sineValue;
        camHolder.transform.localRotation = Quaternion.Euler(0, 0, newPosition);
    }
    public void SetPlayerMotionActive(bool active)
    {
        _enableWalkMotion = active;
    }
}
