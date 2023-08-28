using UnityEngine;
using TetraCreations.Attributes;

namespace EL.Player.Motion
{
    public class CameraShake : MonoBehaviour
    {
        [Title("Shake Settings", TitleColor.Aqua, TitleColor.Brown, 1, 20)]
        [Space]
        [SerializeField] private float shakeDuration = 0.1f; [Tooltip("Duration of the shake effect")]
        [SerializeField] private float shakeAmount = 0.1f; [Tooltip("Intensity of the shake effect")]

        [Space]
        public bool startShake = false;

        private Vector3 originalPosition;
        private Quaternion originalRotation;
        void Start()
        {
            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;
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
}