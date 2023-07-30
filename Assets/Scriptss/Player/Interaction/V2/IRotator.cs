using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRotator : MonoBehaviour, IInteractable
{
    [SerializeField] enum RotationMode
    {
        lerp,
        slerp
    }
    [SerializeField] RotationMode rotationMode;
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform doorTransform;
    [SerializeField] Vector3 targetRotation;
    [SerializeField] Vector3 originalRotation;    

    private bool isRotating = false;
    private bool toggleRotation;

    private void Start() 
    {
        rotationMode = RotationMode.lerp;
    }
    public void Interact()
    { 
        toggleRotation = !toggleRotation;

        if(!isRotating) 
        {
            if (toggleRotation)
            {
              StartCoroutine(RotateObject(targetRotation));  
            } 
            else if(!toggleRotation) 
            {
                StartCoroutine(RotateObject(originalRotation));  
            }
        }
    }
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

}
