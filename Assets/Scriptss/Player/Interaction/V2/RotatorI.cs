using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorI : MonoBehaviour, Iinteractable
{
    [SerializeField] enum RotationMode { lerp, slerp }
    [SerializeField] RotationMode rotationMode;
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform doorTransform;
    [SerializeField] Vector3 targetRotation;
    [SerializeField] Vector3 originalRotation;    

    private bool isRotating = false;
    private bool toggleRotation = true;
    IEnumerator RotateObject(Vector3 targetRotationC)
    {
        isRotating = true;
        Quaternion startRotation = doorTransform.rotation;
        Quaternion endRotation = Quaternion.Euler(targetRotationC);

        float time = 0f;
        while (time < 1f)
        {
            if(rotationMode == RotationMode.lerp) 
            {
                time += Time.deltaTime * rotationSpeed;
                doorTransform.rotation = Quaternion.Lerp(startRotation, endRotation, time);
                yield return null;
            }
            else if(rotationMode == RotationMode.slerp)
            {
                time += Time.deltaTime * rotationSpeed;
                doorTransform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
                yield return null;
            }
        }

        isRotating = false;
    }

    public void OnInteract(Vector3 v)
    {
        toggleRotation = !toggleRotation;

        if (!isRotating)
        {
            StartCoroutine(RotateObject(toggleRotation ? targetRotation : originalRotation));
        }
    }
}
