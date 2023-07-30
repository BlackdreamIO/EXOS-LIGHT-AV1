using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{   
    
    public static Player instance;

//--------------------------------------------------------------------------------------//

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 6.0f;
    [SerializeField] private float sprintSpeed = 12f;
    [SerializeField] private float crouchSpeed = 3.0f;
    [SerializeField] private float crouchMoveSpeed = 3f;
    [SerializeField] private float defaultHeight = 3.0f;
    [SerializeField] private float crouchHeight = 3.0f;
    [SerializeField] private float moveSmoothTime = 0.3f;
    [SerializeField] private float gravity = -30f;
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] float speedIncrementRate;
    [SerializeField] float speedDecrementRate;

    [Header("Stamina Settings")]
    [Range(0, 100)] [SerializeField] private float stamina = 20f;
    [Range(0, 100)] [SerializeField] private float reduceStaminaBy = 2f;
    [Range(0, 100)] [SerializeField] private float idleStaminaIncrease = 5f;
    [Range(0, 100)] [SerializeField] private float walkingStaminaIncrease = 2f;
    [Range(0, 100)] [SerializeField] private float staminaRegainDelay = 2f;
    [Range(0, 100)] [SerializeField] private float minimumStamina = 5f;

    [Space] [Header("MOUSE")]
    [Range(0.0f, 0.5f)] [SerializeField] float mouseSmoothTime = 0.03f;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float mouseClamp = 90f;
    public Camera playerCamera;

    [Space] [Header("INPUT")]
    [SerializeField] KeyCode JumpKey = KeyCode.Space;
    [SerializeField] KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    [Space] [Header("Checking")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;  

    [Space] [Header("SCIRPTS")]
    public HeadBOB headBOB;

    // Script Variables
    private CharacterController controller;
    private Vector2 currentMouseDelta, currentMouseDeltaVelocity;
    private Vector2 currentDir, currentDirVelocity, velocity;
    private bool isGrounded;
    private bool hasStamina;
    private bool crouch;
    private float cameraCap;
    private float velocityY;
    private float originalMoveSpeed;
    private float regainStamina;
    private float increaseStaminaBy;

    [HideInInspector] public bool disableCameraMovemenBool = false;

    [HideInInspector] public float currentStamina;

    [HideInInspector] public bool IsWalking => currentDir != Vector2.zero;
    [HideInInspector] public bool IsStanding => currentDir == Vector2.zero;
    [HideInInspector] public bool IsRunning => moveSpeed == sprintSpeed;
    [HideInInspector] public bool IsCoruching => controller.height == crouchHeight;

//--------------------------------------------------------------------------------------//
    void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.height = defaultHeight;
        originalMoveSpeed = moveSpeed;
        currentStamina = stamina;
        regainStamina = staminaRegainDelay;
        
        playerCamera = Camera.main;

        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }
    void Update()
    {
        if(!disableCameraMovemenBool)
        {
            UpdateMove();
            UpdateMouse();
            HandleStamina();
            HandleCrouch();
        }
        UpdateUI();
    }
    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
 
        cameraCap -= currentMouseDelta.y * mouseSensitivity;
        cameraCap = Mathf.Clamp(cameraCap, -mouseClamp, mouseClamp);
        playerCamera.transform.localEulerAngles = Vector3.right * cameraCap;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }
    void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);
 
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        targetDir.Normalize();
 
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
 
        velocityY += gravity * 2f * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * moveSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
 
        if (isGrounded && Input.GetKeyDown(JumpKey)) { velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity); }
 
        if(isGrounded! && controller.velocity.y < -1f) { velocityY = -8f; }

    }

    void HandleStamina()
    {
        if (Input.GetKey(runKey) && Input.GetKey(KeyCode.W) && !crouch)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                return;
            }
            regainStamina = staminaRegainDelay;

            bool hasStamina = ConsumeStamina();

            if (hasStamina)
            {
                StartCoroutine(IncreaseMoveSpeed());
            }
            else
            {
                Debug.Log("NOT RUNNING");
                StopCoroutine(IncreaseMoveSpeed());
                StartCoroutine(DecreaseMoveSpeed());
            }
        }
        else if (!Input.GetKey(runKey) || !Input.GetKey(KeyCode.W) || crouch)
        {
            regainStamina -= Time.deltaTime;
            regainStamina = Mathf.Clamp(regainStamina, 0, staminaRegainDelay);

            bool startRegain = regainStamina < 1;

            if (startRegain)
            {
                UpdateStaminaIncreaseRate();

                if (currentStamina < stamina)
                {
                    currentStamina += increaseStaminaBy * Time.deltaTime;
                    currentStamina = Mathf.Clamp(currentStamina, 0, stamina);
                }

                if (currentStamina > 1)
                {
                    regainStamina += Time.deltaTime;

                    bool hasStamina = currentStamina > minimumStamina;

                    if (regainStamina > staminaRegainDelay)
                    {
                        regainStamina = staminaRegainDelay;
                        return;
                    }
                }
            }

            StartCoroutine(DecreaseMoveSpeed());
        }
    }
    void UpdateStaminaIncreaseRate()
    {
        if(IsStanding) 
        {
            increaseStaminaBy = idleStaminaIncrease;
        }
        else if(IsWalking) 
        {
            increaseStaminaBy = walkingStaminaIncrease;
        }
    }
    bool ConsumeStamina()
    {
        currentStamina -= reduceStaminaBy * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, stamina);
        hasStamina = currentStamina < minimumStamina ? false : true;
        return hasStamina;
    }
    IEnumerator IncreaseMoveSpeed()
    {
        while (moveSpeed < sprintSpeed && hasStamina)
        {
            moveSpeed += speedIncrementRate * Time.deltaTime;
            moveSpeed = Mathf.Clamp(moveSpeed, 0f, sprintSpeed);
            yield return null;
        }
    }
    IEnumerator DecreaseMoveSpeed()
    {
        if(!crouch) {moveSpeed = originalMoveSpeed;}
        yield return new WaitForSeconds(0.001f);
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            if (controller.height == defaultHeight)
            {
                StartCoroutine(SmoothCrouch(defaultHeight, crouchHeight));
                moveSpeed = crouchMoveSpeed;
                crouch = true;
            }
            else if (controller.height == crouchHeight)
            {
                StartCoroutine(SmoothCrouch(crouchHeight, defaultHeight));
                moveSpeed = originalMoveSpeed;
                crouch = false;
            }
        }
    }
    IEnumerator SmoothCrouch(float startHeight, float targetHeight)
    {
        float elapsedTime = 0f;
        float duration = crouchSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            controller.height = Mathf.Lerp(startHeight, targetHeight, t);

            yield return null;
        }

        controller.height = targetHeight;
    }

    public void DisableMouseMovement()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        disableCameraMovemenBool = true;
    }
    public void EnableMouseMovement()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        disableCameraMovemenBool = false;
    }
    void UpdateUI() 
    {
        PlayerUIManager.instance.UI_staminaSlider.maxValue = stamina;
        PlayerUIManager.instance.UI_staminaSlider.value = currentStamina;
    }
}
