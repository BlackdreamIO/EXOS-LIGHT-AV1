using UnityEngine;
using EL.Core.Player;
public class HeadBOB : MonoBehaviour
{
    [SerializeField] HeadBOBConfigure[] headBOBConfigure;
    [SerializeField] Player playerScript;
    
    [SerializeField] Transform _camera, _cameraHolder;

    private float currentFrequercy;
    private float currentAmount;
    private float currentSmooth;
    private int index;

    private float timer = 0.0f;

    private float _toggleSpeed = 3.0f;
    private Vector3 _startPos;
    private CharacterController _controller;

    bool _enableHeadBob = true;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _startPos = _camera.localPosition;
    }

    void Update()
    {
        if(_enableHeadBob) {
            CheckForHeadBobTrigger();
            UpdatePerameter();
            index = GetPlayerMovementIndex();
        }
    }
    private void UpdatePerameter()
    {
        for (int i = 0; i < headBOBConfigure.Length; i++)
        {
            if(i == index)
            {
                currentFrequercy = headBOBConfigure[i]._frequency;
                currentAmount = headBOBConfigure[i]._amplitude;
                currentSmooth = headBOBConfigure[i]._smooth;
            }
        }
    }

    private int GetPlayerMovementIndex()
    {
        int i;

        switch (playerScript.playerState)
        {
            case Player.PlayerState.idle:
                i = 0;
                break;
            case Player.PlayerState.walking:
                i = 1;
                break;
            case Player.PlayerState.running:
                i = 2;
                break;
            case Player.PlayerState.crouching:
                i = 3;
                break;
            default:
                i = 0;
                break;
        }

        return i;
    }
    private void CheckForHeadBobTrigger()
    {
        float playerMagnitude = new Vector3(playerScript.playerInputX, 0, playerScript.playerInputY).magnitude;

        if (playerMagnitude > 0)
        {
            StartHeadBOB();
        }
    }
    private Vector3 StartHeadBOB()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * currentFrequercy) * currentAmount * 1.4f, currentSmooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Sin(Time.time * currentFrequercy / 2f) * currentAmount * 1.6f, currentSmooth * Time.deltaTime);
        transform.localPosition += pos;
        return pos;
    }
    public void SetHeadBobActivation(bool active)
    {
        _enableHeadBob = active;
    }
}
