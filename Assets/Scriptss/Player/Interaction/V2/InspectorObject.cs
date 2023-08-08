using UnityEngine;
using EL.Player;
public class InspectorObject : MonoBehaviour, Iinspectable
{
    [SerializeField] Transform InspectObject;
    [SerializeField] float m_lerpSpeed = 1f;
    [SerializeField] float m_rotationSpeed = 3f;

    private float m_distance;
    private bool toggle = false;
    private bool canInspect;
    private Transform InspectTransforHolder;
    Transform positionB => InspectTransforHolder;
    private Transform oldParent;
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    private Quaternion orignalRotation;

    private void Start()
    {
        originalPosition = InspectObject.transform.position;
        orignalRotation = InspectObject.transform.localRotation; 
        InspectObject = this.transform;

        if(oldParent == null && InspectObject.parent)
        {
            oldParent = InspectObject.parent;
        }
     }

    private void Update()
    {  
        InspectObjectPositionManager();
        RotationForInspectObject();
        ResetRotation();
    }
    public void Inspect(Transform transform)
    {
        toggle = !toggle;
        PlayerActionManager();
        InspectTransforHolder = transform;
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
    private void PlayerActionManager()
    {
        if(toggle) 
        {
            //Player.instance.DisableMouseMovement();
        }
        else if(!toggle)
        {
            //Player.instance.EnableMouseMovement();
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
}