using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    //public static PlayerCameraController instence;

    [SerializeField] Transform player;

    [SerializeField] Camera cam;
    [SerializeField] float mouseSensitivity = 3.0f;
    [SerializeField] float Roatation_X = 90;
    private float verticalRotation = 0f;
    private float horizontalRotation;

    public bool _enable_camera = true;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (_enable_camera)
        {
            LockedCamera();
        }
        else
        {
            UnlockedCamera();
        }
    }
    private void LockedCamera()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -Roatation_X, Roatation_X);
        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        player.Rotate(0, horizontalRotation, 0);
    }
    private void UnlockedCamera()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetPlayerCamActive(bool active)
    {
        _enable_camera = active;
    }
}

