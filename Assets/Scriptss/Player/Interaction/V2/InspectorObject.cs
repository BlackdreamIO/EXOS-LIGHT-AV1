using UnityEngine;
using EL.Core.PlayerAcessPointer;
public class InspectorObject : MonoBehaviour, Iinspectable
{   
    public string ID;
    [SerializeField] Transform InspectObject; 
    [SerializeField] private float m_lerpSpeed = 1f;
    [SerializeField] private float m_rotationSpeed = 3f;
    [SerializeField] Outline outline;

    #region SCRIPT VARIABLE

    private float m_distance;
    private bool toggle = false;
    private bool canInspect;
    private Transform InspectTransforHolder;
    Transform positionB => InspectTransforHolder;
    private Transform oldParent;
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    private Quaternion orignalRotation;

    private PlayerAccessPoint playerAccessPoint;

    #endregion
    private void Start()
    {
        // save the orignal position and rotation of this object
        originalPosition = InspectObject.transform.position;
        orignalRotation = InspectObject.transform.localRotation;

        // set the InspectObject to this obejct 
        InspectObject = this.transform;

        // check if already has a parent then set the old parent to current parent
        if (oldParent == null && InspectObject.parent)
        {
            oldParent = InspectObject.parent;
        }

        playerAccessPoint = PlayerAccessPoint.Instance;
        if(outline == null)
        {
            try
            {
                outline = GetComponent<Outline>();
            }
            catch (System.Exception)
            {
                Debug.Log("Couldn't Find Any Component As Outline");
                throw;
            }
        }
        //canvas = GetComponentInChildren<Canvas>();
     }

    private void Update()
    {
        InspectObjectPositionManager();
        RotationForInspectObject();
        ResetRotation();
    }
    public void Inspect(Transform transform)
    {
        toggle =! toggle;
        InspectTransforHolder = transform;
        if(toggle)
        {
            outline.enabled = false;
        }
        else
        {
            outline.enabled = true;
        }

        UpdatePlayerAction();
    }


    private void InspectObjectPositionManager()
    {
        if(positionB != null)
        {
            InspectObject.SetParent(InspectTransforHolder);
            m_distance = Vector3.Distance(InspectObject.transform.position, targetPosition);

            if(InspectObject.parent == InspectTransforHolder) 
            {
                targetPosition = toggle ? positionB.position : originalPosition;
                InspectObject.position = Vector3.Lerp(InspectObject.transform.position, targetPosition, m_lerpSpeed * Time.deltaTime);
            }

            canInspect = m_distance < 1 || m_distance < 0.5;
        }
    }

    private void ResetRotation()
    {
        if (!toggle && InspectObject.parent == InspectTransforHolder)
        {
            if (oldParent != null)
            {
                InspectObject.SetParent(oldParent);
                Quaternion A = InspectObject.transform.localRotation;
                InspectObject.transform.localRotation = Quaternion.Lerp(A, orignalRotation, (m_lerpSpeed + 2) * Time.deltaTime);
            }
            else
            {
                InspectObject.SetParent(oldParent);
                Quaternion A = InspectObject.transform.localRotation;
                InspectObject.transform.localRotation = Quaternion.Lerp(A, orignalRotation, (m_lerpSpeed + 2) * Time.deltaTime);
            }
        }
    }
    private void RotationForInspectObject()
    {
        if(canInspect && Input.GetMouseButton(0)) 
        {
            float XaxisRotation = Input.GetAxis("Mouse X") * m_rotationSpeed;
            float YaxisRotation = Input.GetAxis("Mouse Y") * m_rotationSpeed;

            InspectObject.transform.Rotate(Vector3.up, XaxisRotation);
            InspectObject.transform.Rotate(Vector3.forward, YaxisRotation);
        }
    }
    private void UpdatePlayerAction()
    {
        if (toggle)
        {
            playerAccessPoint.sendRequest = PlayerAccessPoint.SendRequest.DoInteraction;
            playerAccessPoint.UpdatePlayerAction();
        }
        else if (!toggle)
        {
            playerAccessPoint.sendRequest = PlayerAccessPoint.SendRequest.DoMovement;
            playerAccessPoint.UpdatePlayerAction();
        }
    }
}