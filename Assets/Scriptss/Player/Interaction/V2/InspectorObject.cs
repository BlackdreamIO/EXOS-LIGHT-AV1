using UnityEngine;
using EL.Core.PlayerAcessPointer;

public class InspectorObject : MonoBehaviour
{
    [Header("Inspector Settings")]
    public string ID;
    //[SerializeField] Transform InspectObject;
    [SerializeField] private float m_lerpSpeed = 1f;
    [SerializeField] private float m_rotationSpeed = 3f;
    [SerializeField] Outline outline;

    #region SCRIPT VARIABLE

    private float m_distance;
    private bool isInspecting = false;
    private bool canInspect;
    private Transform InspectTransforHolder;
    Transform positionB => InspectTransforHolder;
    private Transform oldParent;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Vector3 currentVelocity;

    private Quaternion orignalRotation;

    private PlayerAccessPoint playerAccessPoint;

    #endregion
    private void Start()
    {
        // save the orignal position and rotation of this object
        originalPosition = transform.position;
        orignalRotation = transform.localRotation;

        // set the InspectObject to this obejct 
        //InspectObject = transform;

        // check if already has a parent then set the old parent to current parent
        if (oldParent == null && transform.parent)
        {
            oldParent = transform.parent;
        }

        playerAccessPoint = PlayerAccessPoint.Instance;
    }

    private void Update()
    {
        ManageInspectPosition();
        ManageRotation();
        ResetRotation();
    }
    public void Inspect(Transform transform)
    {
        isInspecting =! isInspecting;
        InspectTransforHolder = transform;

        outline.enabled =  isInspecting ? false : true;

        UpdatePlayerAction();
    }
    private void ManageInspectPosition()
    {
        if (positionB != null)
        {
            transform.SetParent(InspectTransforHolder);
            m_distance = Vector3.Distance(transform.position, targetPosition);

            if (transform.parent == InspectTransforHolder)
            {
                targetPosition = isInspecting ? positionB.position : originalPosition;
                transform.position = Vector3.Lerp(transform.position, targetPosition,  m_lerpSpeed * Time.deltaTime);
            }

            canInspect = m_distance < 1 || m_distance < 0.5;
        }
    }
    private void ManageRotation()
    {
        if (canInspect && Input.GetMouseButton(0))
        {
            float YaxisRotation = Input.GetAxis("Mouse X") * m_rotationSpeed;
            float XaxisRotation = Input.GetAxis("Mouse Y") * m_rotationSpeed;

            transform.Rotate(Vector3.up, YaxisRotation);
            transform.Rotate(Vector3.forward, XaxisRotation);
        }
    }
    private void ResetRotation()
    {
        if (!isInspecting && transform.parent == InspectTransforHolder)
        {
            if (oldParent != null)
            {
                transform.SetParent(oldParent);
                Quaternion A = transform.transform.localRotation;
                transform.transform.localRotation = Quaternion.Slerp(A, orignalRotation, (m_lerpSpeed + 2) * Time.deltaTime);
            }
            else
            {
                transform.SetParent(oldParent);
                Quaternion A = transform.transform.localRotation;
                transform.localRotation = Quaternion.Slerp(A, orignalRotation, (m_lerpSpeed + 2) * Time.deltaTime);
            }
        }
    }
    private void UpdatePlayerAction()
    {
        if (isInspecting)
        {
            playerAccessPoint.sendRequest = PlayerAccessPoint.SendRequest.DoInteraction;
            playerAccessPoint.UpdatePlayerAction();
            playerAccessPoint.GetPlayerUIComponent().ShowInspectPanel(ID);
        }
        else
        {
            playerAccessPoint.sendRequest = PlayerAccessPoint.SendRequest.DoMovement;
            playerAccessPoint.UpdatePlayerAction();
            playerAccessPoint.GetPlayerUIComponent().HideInspectPanel();
        }
    }

}
