using UnityEngine;
using System.Collections;
using EL.Player.Motion;
using TetraCreations.Attributes;

namespace EL.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {   
        public static Player instance;

        //--------------------------------------------------------------------------------------//

        #region MOVEMENT VARIABLES

        [Title("MOVEMENT", TitleColor.Cyan, TitleColor.White, 1f, 20f)]

        [SerializeField] bool EditMovementSetting;
        [DrawIf(nameof(EditMovementSetting), true, DisablingType.DontDraw)] [SerializeField] float moveSpeed = 6.0f;
        [DrawIf(nameof(EditMovementSetting), true, DisablingType.DontDraw)] [SerializeField] float sprintSpeed = 12f;
        [DrawIf(nameof(EditMovementSetting), true, DisablingType.DontDraw)] [SerializeField] float smoothTime = 12f;
        [DrawIf(nameof(EditMovementSetting), true, DisablingType.DontDraw)] [SerializeField] float m_crouchSmoothTime = 3.0f;
        [DrawIf(nameof(EditMovementSetting), true, DisablingType.DontDraw)] [SerializeField] float m_crouchMoveSpeed = 3f;
        [DrawIf(nameof(EditMovementSetting), true, DisablingType.DontDraw)] [SerializeField] float m_defaultScale = 3.0f;
        [DrawIf(nameof(EditMovementSetting), true, DisablingType.DontDraw)] [SerializeField] float m_crouchScale = 3.0f;
        [DrawIf(nameof(EditMovementSetting), true, DisablingType.DontDraw)][SerializeField] float gravity = 20.0f;

        #endregion

        #region STAMINA VARIABLES

        [Title("STAMINA", TitleColor.Aqua, TitleColor.BlueVariant, 1f, 20f)]

        [SerializeField] bool EditStaminaSetting;

        [DrawIf(nameof(EditStaminaSetting), true, DisablingType.DontDraw)] [SerializeField] float m_Stamina;
        [DrawIf(nameof(EditStaminaSetting), true, DisablingType.DontDraw)] [SerializeField] float m_minStamina;
        [DrawIf(nameof(EditStaminaSetting), true, DisablingType.DontDraw)] [SerializeField] float m_StaminaReduce;
        [DrawIf(nameof(EditStaminaSetting), true, DisablingType.DontDraw)] [SerializeField] float m_StaminaIDelay;
        [DrawIf(nameof(EditStaminaSetting), true, DisablingType.DontDraw)] [SerializeField] float m_IdleStaminaIncrease;
        [DrawIf(nameof(EditStaminaSetting), true, DisablingType.DontDraw)] [SerializeField] float m_WalkStaminaIncrease;
        [DrawIf(nameof(EditStaminaSetting), true, DisablingType.DontDraw)] [SerializeField] float m_RunStaminaIncrease;
        [DrawIf(nameof(EditStaminaSetting), true, DisablingType.DontDraw)] [SerializeField] float m_CrouchStaminaIncrease;

        #endregion

        #region INPUT VARIABLES

        [Title("INPUT", TitleColor.Yellow, TitleColor.White, 1f, 20f)]
        [SerializeField] bool EditInputSetting;
        [DrawIf(nameof(EditInputSetting), true, DisablingType.DontDraw)] [SerializeField] KeyCode JumpKey = KeyCode.Space;
        [DrawIf(nameof(EditInputSetting), true, DisablingType.DontDraw)] [SerializeField] KeyCode runKey = KeyCode.LeftShift;
        [DrawIf(nameof(EditInputSetting), true, DisablingType.DontDraw)] [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

        #endregion

        public float mouseSensitivity = 2.0f;
        private float verticalRotation = 0f;
        private Camera playerCamera;
        // Script Variables
        private CharacterController characterController;
        private float originalMoveSpeed;

        private bool IsWalking;
        private bool IsIdle;
        private bool IsRunning;
        private bool IsCrouching;

        private float m_currentMoveSpeed;
        private Vector3 m_playerScale;
        private float refVelocity;

        private float m_currentPlayerHeight;
        private float ref_crouchVelocity;

        private float m_currentStamina;

        [HideInInspector] public bool disableCameraMovemenBool = false;
        public enum PlayerState 
        {
            idle,
            walking,
            running,
            crouching
        }
        public PlayerState playerState = PlayerState.idle;

        //--------------------------------------------------------------------------------------//
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            m_currentPlayerHeight = m_defaultScale;

            originalMoveSpeed = moveSpeed;
            m_currentMoveSpeed = moveSpeed;

            if (instance == null) { instance = this; }
            else { Destroy(gameObject); }

            //, SetUp Stamina Value
            m_currentStamina = m_Stamina;
            m_playerScale = this.transform.localScale;

            playerCamera = GetComponentInChildren<Camera>();
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void Update()
        {
            UpdateUI();
            if(disableCameraMovemenBool) {return;}
            //HandleCrouch();
            PlayerStateController();
            HandleMovement();
        }

        private void PlayerStateController() 
        {
            Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            if (IsIdle)
            {
                playerState = PlayerState.idle;
            }
            else if(IsWalking && !IsRunning)
            {
                playerState = PlayerState.walking;
            }
            else if (IsRunning)
            {
                playerState = PlayerState.running;
            }
            else if(IsCrouching)
            {
                playerState = PlayerState.crouching;
            }
        }
        private void HandleMovement()
        {
            Movement();
            SprintMovement();
            CrouchMovement();

            bool CanRun() { return m_currentStamina > m_minStamina; }

            #region Player Movement

            void Movement()
            {
                // Mouse Look
                float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
                transform.Rotate(0, horizontalRotation, 0);

                verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
                verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
                playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

                // Player Movement
                float moveX = Input.GetAxis("Vertical") * m_currentMoveSpeed;
                float moveY = Input.GetAxis("Horizontal") * m_currentMoveSpeed;

                Vector2 moveVector = new Vector2(moveX, moveY);
                Vector3 moveDir = transform.TransformDirection(new Vector3(moveY, 0, moveX));

                if (!characterController.isGrounded)
                {
                    moveDir.y -= (gravity * 2) * Time.deltaTime;
                }

                characterController.Move(moveDir * Time.deltaTime);

                if (moveVector.magnitude == 0)
                {
                    IsIdle = true;
                    IsWalking = false;
                }
                else
                {
                    IsIdle = false;
                    IsWalking = true;
                }
            }

            #endregion

            #region Player Sprint Movement

            void SprintMovement()
            {
                if(Input.GetKey(runKey) && Input.GetKey(KeyCode.W) && CanRun())
                {   
                    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) { return; }

                    m_currentMoveSpeed = Mathf.SmoothDamp(m_currentMoveSpeed, sprintSpeed, ref refVelocity, smoothTime);
                    m_currentStamina -= m_StaminaReduce * Time.deltaTime;
                    m_currentStamina = Mathf.Clamp(m_currentStamina, m_minStamina, m_Stamina);
                    if(m_currentStamina < m_minStamina) { m_currentStamina = m_minStamina; }
                    IsRunning = true;
                }
                else if (!Input.GetKey(runKey) || !CanRun())
                {
                    m_currentMoveSpeed = Mathf.SmoothDamp(m_currentMoveSpeed, originalMoveSpeed, ref refVelocity, smoothTime);

                    // Increase Stamina Depeneding On Player Action
                    switch (playerState)
                    {
                        case PlayerState.idle:
                            m_currentStamina += m_IdleStaminaIncrease * Time.deltaTime;
                            break;
                        case PlayerState.walking:
                            m_currentStamina += m_WalkStaminaIncrease * Time.deltaTime;
                            break;
                        case PlayerState.running:
                            m_currentStamina += m_RunStaminaIncrease * Time.deltaTime;
                            break;
                        case PlayerState.crouching:
                            m_currentStamina += m_CrouchStaminaIncrease * Time.deltaTime;
                            break;
                        default:
                            m_currentStamina += m_IdleStaminaIncrease * Time.deltaTime;
                            break;
                    }
                    m_currentStamina = Mathf.Clamp(m_currentStamina, m_minStamina, m_Stamina);
                    if (m_currentStamina > m_Stamina) { m_currentStamina = m_Stamina; }
                    IsRunning = false;
                }
            }

            #endregion

            #region Player Crouch Movement
            
            void CrouchMovement()
            {
                float yScale = Mathf.SmoothDamp(this.transform.localScale.y, IsCrouching ? m_crouchScale : m_defaultScale, ref ref_crouchVelocity, m_crouchSmoothTime);
                Mathf.Ceil(yScale);
                this.transform.localScale = new Vector3(this.transform.localScale.x, yScale, this.transform.localScale.z);

                if (Input.GetKeyDown(crouchKey))
                { IsCrouching = !IsCrouching; }
            }

            #endregion
        }
    
        void UpdateUI() 
        {
            PlayerUIManager.instance.UI_staminaSlider.maxValue = m_Stamina;
            PlayerUIManager.instance.UI_staminaSlider.value = m_currentStamina;
        }
    }    
}
