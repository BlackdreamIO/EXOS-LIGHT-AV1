using UnityEngine;
using EL.Core.Player;
using EL.Core.PlayerAcessPointer;
using System.Collections;

public class HandRotation : MonoBehaviour
{
    [SerializeField] float m_intensity = 2f; // Sensivity for the hand 
    [SerializeField] float m_smooth = 2f;
    [SerializeField] float clampX;
    [SerializeField] float clampY;
    [SerializeField] float idleTime = 3f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private Quaternion originalRotation;
    private bool isRotating = false;

    private PlayerAccessPoint playerAccessPoint;

    private float currentTimer = 0f;
    private bool alreadyCalledPAP = false;
    private void Start()
    {
        originalRotation = this.transform.rotation;
        playerAccessPoint = PlayerAccessPoint.Instance;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMouseInput();
        ManageHandMovement();
        UpdatePlayerAction();

        originalRotation = playerAccessPoint.GetPlayerComponent().transform.rotation;
    }

    private void HandleMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX -= mouseY * m_intensity;
        rotationY += mouseX * m_intensity;

        rotationX = Mathf.Clamp(rotationX, -clampX, clampX);
        rotationY = Mathf.Clamp(rotationY, -clampY, clampY);
    }
    private void ManageHandMovement()
    {
        Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0f);

        Quaternion A = transform.rotation;
        Quaternion B = targetRotation;

        if (Input.GetMouseButton(0))
        {
            isRotating = true;
            transform.rotation = Quaternion.Lerp(A, B, m_smooth * Time.deltaTime);
        }
        else
        {
            isRotating = false;
            CheckIdleTime();
        }
    }
    private void CheckIdleTime()
    {
        // if our player state is idle and we are not rotating our hand
        if(playerAccessPoint.GetPlayerComponent().playerState == Player.PlayerState.idle )
        {
            currentTimer += Time.deltaTime; // we will increase our float current time by delta time 

            if (currentTimer > idleTime) // if we go higher then idleTime 
            {
                Debug.Log("Player Has Been Waiting For " + idleTime);
                StartCoroutine(ResetHandRotation()); // start our reset coroutine
                currentTimer = 0f; // then current timer to 0
            }
        }
    }

    private IEnumerator ResetHandRotation()
    {
        float timer = 0f;

        Quaternion startRotation = transform.rotation; // we will store our current rotation as startRotation
        Quaternion targetRotation = originalRotation; // and originalRotation to targetRotation

        while (timer < idleTime) // if our current time is less then idle time 
        {
            timer += Time.deltaTime; // we will increase it by delta time until timer > idleTime

            float t = Mathf.Clamp01(timer / idleTime); // Normalize time between 0 and 1

            // we will linearly interpolate between our startRotation to targetRotation by t
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t); 

            if (timer >= idleTime || isRotating) // if we reach out our idle time or if we go above idleTime
            {
                transform.rotation = targetRotation; // Ensure the final rotation is exact
                break;
            }
            yield return null;
        }
    }

    private void UpdatePlayerAction()
    {
        if(isRotating) // if we are rotating the hand 
        {
            if(!alreadyCalledPAP) // and alreadyCalledPAP is not called
            {
                Debug.Log("Disable Camera"); // then we will disable our camera Movement

                playerAccessPoint.sendRequest = PlayerAccessPoint.SendRequest.DoHandMovement;
                playerAccessPoint.UpdatePlayerAction();

                alreadyCalledPAP = true; // and set the alreadyCalledPAP to true
            }
        }
        else if(!isRotating) // otherwise if we are not rotating our hand
        {
            if(alreadyCalledPAP) // and we have already called alreadyCalledPAP
            {
                Debug.Log("Enable Camera"); // then we will this time enable the camera

                playerAccessPoint.sendRequest = PlayerAccessPoint.SendRequest.DoMovement;
                playerAccessPoint.UpdatePlayerAction();

                alreadyCalledPAP = false; // also we will set the alreadyCalledPAP = false
            }
        }
        
    }
}
