using UnityEngine;
using EL.Core.Player;
using TetraCreations.Attributes;
public class BodyCamHand : MonoBehaviour
{
    [Title("BodyCamHand",TitleColor.Aqua, TitleColor.Beige, 2, 20)]
    [SerializeField] Player playerScript;


    [Header("Walk Configuration"),Space(20)]
    [SerializeField] float WalkFrequency = 15;
    [SerializeField] private float WalkAmount = 2;

    [Header("Run Configuration"), Space(20)]
    [SerializeField] float RunFrequency = 30;
    [SerializeField] private float RunAmount = 4f;

    [Header("Crouch Configuration"), Space(20)]
    [SerializeField] float CrouchFrequency = 30;
    [SerializeField] private float CrouchAmount = 4f;


    private float p_currentFrequency = 15;
    private float p_currentAmount = 2;
    private float currentVelocity;

    private void Update()
    {
        float playerMagnitude = new Vector3(playerScript.playerInputX, 0, playerScript.playerInputY).magnitude;

        if (playerMagnitude > 0)
        {
            StartBOB();
        }
        UpdatePerameter();
    }
    private void UpdatePerameter()
    {
        switch (playerScript.playerState)
        {
            case Player.PlayerState.walking:
                p_currentFrequency = WalkFrequency;
                p_currentAmount = WalkAmount;
                break;
            
            case Player.PlayerState.running:
                p_currentFrequency = RunFrequency;
                p_currentAmount = RunAmount;
                break;

            case Player.PlayerState.crouching:
                p_currentFrequency = CrouchFrequency;
                p_currentAmount = CrouchAmount;
                break;

            default:
                break;
        }
    }
    private Vector3 StartBOB() // Hand Bob Effect For Walking Or Running
    {
        Vector3 pos = Vector3.zero;
        pos.y = Mathf.Sin(Time.time * p_currentFrequency) * p_currentAmount * 1.4f;
        pos.x = Mathf.Sin(Time.time * p_currentFrequency / 2f) *p_currentAmount * 1.6f;

        transform.localPosition = pos;
        return pos;
    }
}
