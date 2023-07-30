using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuuningEffect : MonoBehaviour
{
    public float shakeDuration = 0.1f; // Duration of the shake effect
    public float shakeAmount = 0.1f; // Intensity of the shake effect

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }
    private void Update() {
        ShakeCamera();
    }
    public void ShakeCamera()
    {
        if (shakeDuration > 0)
        {
            // Apply random displacements to the camera's position and rotation
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;
            transform.localRotation = new Quaternion(
                originalRotation.x + Random.Range(-shakeAmount, shakeAmount) * 0.2f,
                originalRotation.y + Random.Range(-shakeAmount, shakeAmount) * 0.2f,
                originalRotation.z + Random.Range(-shakeAmount, shakeAmount) * 0.2f,
                originalRotation.w + Random.Range(-shakeAmount, shakeAmount) * 0.2f
            );

            shakeDuration -= Time.deltaTime;
        }
        else
        {
            // Reset the camera's position and rotation
            shakeDuration = 0f;
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
        }
    }
}