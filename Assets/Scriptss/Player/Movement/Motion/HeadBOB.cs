using UnityEngine;
using EL.Player;

namespace EL.Player.Motion
{
    public class HeadBOB : MonoBehaviour
    {  
        [Space] [Header("WALKING")] [Space]
        [Range(1f, 33f)] public float Frequercy = 10.1f;
        [Range(0.001f, 0.02f)] public float Amount = 0.02f;
        [Range(10f, 100f)] public float Smooth = 10.0f;
        
        [Space] [Header("RUNNING")] [Space]
        [Range(1f, 33f)]        [SerializeField] float Run_Frequercy = 10.1f;
        [Range(0.001f, 0.02f)]  [SerializeField] float Run_Amount = 0.02f;
        [Range(10f, 100f)]      [SerializeField] float Run_Smooth = 10.0f;
        
        [Space] [Header("ADDITIONAL RUNNING EFFECT")] [Space]
        [SerializeField] float speed = 1.0f;
        [SerializeField] float amplitude = 1.0f;
        [SerializeField] float offset = 0.0f;
        [SerializeField] float RUN_INCREAMENT = 1f;

        [Space] [Header("CROUCHING")] [Space]
        [Range(1f, 33f)]        [SerializeField] float Crouch_Frequercy = 10.1f;
        [Range(0.001f, 0.02f)]  [SerializeField] float Crouch_Amount = 0.02f;
        [Range(10f, 100f)]      [SerializeField] float Crouch_Smooth = 10.0f;
        [SerializeField] float CROUCH_INCREAMENT = 1f;


        [Space] [Header("SHAKE SETTINGS")] [Space]
        [SerializeField] private float shakeDuration = 0.1f; [Tooltip("Duration of the shake effect")]
        [SerializeField] private float shakeAmount = 0.1f; [Tooltip("Intensity of the shake effect")] 

        [Space]
        Player player;
        public bool startShake = false;

        #region Scirpts Variable
        float orignalSmooth;
        bool is_intense, is_lowtense;

        // Script Variables
        private float currentFrequercy;
        private float currentAmount;
        private float currentSmooth;

        private Vector3 originalPosition;
        private Quaternion originalRotation;

        #endregion

        private void Start() 
        { 
            orignalSmooth = Smooth; 
            currentFrequercy = Frequercy;
            currentAmount = Amount;
            currentSmooth = Smooth;

            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;
        }

        private void Update() 
        {
            if(!Player.instance.disableCameraMovemenBool)
            {
                CheckForHeadBobTrigger();
                PerformHeadBobManager();
                CheckForRunTrigger();
                CheckForCameraShake();
            }
        }

        private void CheckForCameraShake()
        {
            if(startShake)
            {
                ShakeCamera();
            }
        }
        private void CheckForRunTrigger()
        {
            float time = Time.time * speed;
            float sineValue = Mathf.Sin(time) * amplitude + offset;

            if (player.IsRunning)
            {
                UpdateObjectPosition(sineValue);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        private void CheckForHeadBobTrigger() 
        {
            float playerMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

            if(playerMagnitude > 0) 
            {
                StartHeadBOB();
            }
        }
        private Vector3 StartHeadBOB() 
        {
            Vector3 pos = Vector3.zero;
            pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * currentFrequercy) * currentAmount * 1.4f, currentSmooth * Time.deltaTime);
            pos.x += Mathf.Lerp(pos.x, Mathf.Sin(Time.time * currentFrequercy / 2f) * Crouch_Amount * 1.6f, currentSmooth * Time.deltaTime);
            transform.localPosition += pos;
            return pos;
            
        }
        
        private void PerformHeadBobManager()
        {
            if(player.IsStanding && !player.IsRunning) 
            {   
                PerformHeadBobWhileStanding();
            }
            if(player.IsWalking && !player.IsRunning) 
            {   
                PerformHeadBobWhileWalking();
            }
            if(player.IsRunning)
            {   
                PerformHeadBobWhileRunning();
            }
        }
        public void PerformHeadBobWhileStanding() 
        {
            //Debug.Log("Performing Standing Headbob");
        }
        public void PerformHeadBobWhileWalking() 
        {   
            //Debug.Log("Performing Walking Headbob");
            currentFrequercy = Frequercy;
            currentAmount = Amount;
            currentSmooth = Smooth;
        }
        public void PerformHeadBobWhileRunning() 
        {
            //Debug.Log("Performing Running Headbob");
            currentFrequercy = Run_Frequercy;
            currentAmount = Run_Amount;
            currentSmooth = Run_Smooth;
        }
        public void PerformHeadBobWhileCrouching() 
        {

        }

        private void UpdateObjectPosition(float value)
        {
            float newPosition = transform.localRotation.z;
            newPosition = value;
            transform.localRotation = Quaternion.Euler(0, 0, newPosition);
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