using UnityEngine;
using EL.Player;

namespace EL.Player.Motion
{
    public class HeadBOB : MonoBehaviour
    {  
        
        [Space] [Header("SHAKE SETTINGS")] [Space]
        [SerializeField] private float shakeDuration = 0.1f; [Tooltip("Duration of the shake effect")]
        [SerializeField] private float shakeAmount = 0.1f; [Tooltip("Intensity of the shake effect")] 

        [Space]
        public bool startShake = false;

        public float T;
        public float OM; // original movement
        public float SM; // sprint movement
        public float OPM; // original output movement

        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private float refVelocity;

        private void Start() 
        { 
            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;
        }

        private void Update() 
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                OPM = Mathf.SmoothDamp(OPM, SM, ref refVelocity, T);
            }
            else
            {
                OPM = Mathf.SmoothDamp(OPM, OM, ref refVelocity, T);
            }
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