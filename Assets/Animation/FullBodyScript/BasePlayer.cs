using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    [SerializeField] Transform _player, targetObect;
    [SerializeField] Vector3 lockVetcor;
    [SerializeField] bool lockPosition;

    public enum LockType {
        lockAllAxis,
        lockXOnly,
        lockYOnly,
        lockZOnly,
        FollowTransform
    }
    [SerializeField] LockType lockType = LockType.lockAllAxis;
    void Update()
    {
        if(lockPosition) 
        {
            _player.transform.localPosition = lockVetcor;
        }

        switch (lockType)
        {
            case LockType.lockAllAxis:
                _player.transform.localPosition = lockVetcor;
                break;

            case LockType.lockXOnly:
                _player.transform.localPosition = new Vector3(lockVetcor.x, 0f, 0f);
                break;

            case LockType.lockYOnly:
                _player.transform.localPosition = new Vector3(0f, lockVetcor.y, 0f);
                break;

            case LockType.lockZOnly:
                _player.transform.localPosition = new Vector3(0f, 0f, lockVetcor.z);
                break;

            case LockType.FollowTransform:
                _player.transform.position = targetObect.position;
                break;


            default:
                break;
        }
    }
}
