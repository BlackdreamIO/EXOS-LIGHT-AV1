using UnityEngine;
using TetraCreations.Attributes;

namespace EL.Core.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {   
        //--------------------------------------------------------------------------------------//

        #region MOVEMENT VARIABLES

        [Title("MOVEMENT", TitleColor.Cyan, TitleColor.White, 1f, 20f)]

        [SerializeField] bool EditMovementSetting;
        [DrawIf(nameof(EditMovementSetting), true, DisablingType.DontDraw)] [SerializeField] Transform cameraRoot;
        [DrawIf(nameof(EditMovementSetting), true, DisablingType.DontDraw)] [SerializeField] float moveSpeed = 6.0f;
        [DrawIf(nameof(EditMovementSetting), true, DisablingType.DontDraw)] [SerializeField] float sprintSpeed = 12f;
        [DrawIf(nameof(EditMovementSetting), true, DisablingType.DontDraw)] [SerializeField] float smoothTime = 12f;
       
        [DrawIf(nameof(EditMovementSetting), true, DisablingType.DontDraw)][SerializeField] float gravity = 20.0f;

        #endregion

        #region STAMINA VARIABLES

        [Title("STAMINA", TitleColor.Aqua, TitleColor.BlueVariant, 1f, 20f)]

        [SerializeField] bool EditStaminaSetting;

        [DrawIf(nameof(EditStaminaSetting), true, DisablingType.DontDraw)]  public float m_Stamina;
        [DrawIf(nameof(EditStaminaSetting), true, DisablingType.DontDraw)]  public float m_minStamina;
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

        #region SCIRPTS VARIABLES

        private CharacterController characterController;
        private float originalMoveSpeed;

        private bool IsWalking;
        private bool IsIdle;
        private bool IsRunning;
        private bool IsCrouching;
        private Vector3 originalCenter;
        private float originalHeight;

        private float m_currentMoveSpeed;
        private float refVelocity;

        private float m_currentPlayerHeight;
        private float ref_crouchVelocity;
        private Vector3 orginalCameraRoot;

        private bool enablePlayerMovement = true;
        public enum PlayerState 
        {
            idle,
            walking,
            running,
            crouching
        }
       
        public PlayerState playerState = PlayerState.idle;

        #endregion

        #region HIDE IN INSPECTOR VARIABLES

        [HideInInspector] public float m_currentStamina;
        [HideInInspector] public float playerInputX , playerInputY;

        #endregion
        //--------------------------------------------------------------------------------------//
        private void Start()
        {
            characterController = GetComponent<CharacterController>();

            originalMoveSpeed = moveSpeed;
            m_currentMoveSpeed = moveSpeed;

            originalCenter = characterController.center;
            originalHeight = characterController.height;
            orginalCameraRoot = cameraRoot.localPosition;

            m_currentStamina = m_Stamina;
        }
        private void Update()
        {
            if(enablePlayerMovement)
            {
                PlayerStateController();
                HandleMovement();
            }
        }

        private void PlayerStateController() 
        {
            if (IsIdle && !IsCrouching)
            {
                playerState = PlayerState.idle;
            }
            else if(IsWalking && !IsRunning && !IsCrouching)
            {
                playerState = PlayerState.walking;
            }
            else if (IsRunning)
            {
                playerState = PlayerState.running;
            }
            else if(IsCrouching && IsWalking || IsCrouching)
            {
                playerState = PlayerState.crouching;
            }
        }
        public void HandleMovement()
        {
            Movement();
            SprintMovement();
            CrouchMovement();
        }

        #region Player Movement Handler

            #region  Section : Movement
            void Movement()
            {
            // Player Movement
                playerInputX = Input.GetAxis("Horizontal") * m_currentMoveSpeed;
                playerInputY = Input.GetAxis("Vertical") * m_currentMoveSpeed;

                Vector2 moveVector = new Vector2(playerInputX, playerInputY);
                Vector3 moveDir = transform.TransformDirection(new Vector3(playerInputX, 0, playerInputY));

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

            #region Section : Sprint  
            bool CanRun() { return m_currentStamina > m_minStamina; }
            void SprintMovement()
            {
                if (Input.GetKey(runKey) && Input.GetKey(KeyCode.W) && CanRun() && !IsCrouching)
                {
                    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) { return; }

                    m_currentMoveSpeed = Mathf.SmoothDamp(m_currentMoveSpeed, sprintSpeed, ref refVelocity, smoothTime);
                    m_currentStamina -= m_StaminaReduce * Time.deltaTime;
                    m_currentStamina = Mathf.Clamp(m_currentStamina, m_minStamina, m_Stamina);
                    if (m_currentStamina < m_minStamina) { m_currentStamina = m_minStamina; }
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

            #region Section : Crouch 
            void CrouchMovement()
            {
                if (Input.GetKeyDown(crouchKey)) { IsCrouching =! IsCrouching; }

                cameraRoot.localPosition = Vector3.Lerp(cameraRoot.localPosition, IsCrouching ? new Vector3(0f, -0.5f, 0f) : orginalCameraRoot, 4f * Time.deltaTime);

                characterController.height = IsCrouching ? 1f : originalHeight;
                characterController.center = IsCrouching ? new Vector3(0f, -0.5f, 0f) : originalCenter;
            }

        #endregion

        #endregion

        public void SetPlayerMovementActive(bool active)
        {
            enablePlayerMovement = active;
        }

    }   
}



