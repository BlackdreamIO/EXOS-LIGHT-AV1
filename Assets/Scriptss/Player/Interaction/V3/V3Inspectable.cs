using UnityEngine;
using TetraCreations.Attributes;
using EL.Core.PlayerAcessPointer;

public class V3Inspectable : MonoBehaviour, Iinteractor
{

    [Title("Inspector Settings", TitleColor.Aqua, TitleColor.Brown, 1, 20)]
    [SerializeField] private float m_lerpSpeed = 1f;
    [SerializeField] private float m_rotationSpeed = 3f;
    [SerializeField] private float m_nearDistance = 1f;

    // SCIRPT VARIABLE
    private Vector3 _originalPosition;
    private Vector3 _targetPosition;
    private Quaternion _originalRotation;
    private Transform _currentTransform => transform;

    private bool _canInspect;
    private bool _alreadyCalledPAP = false;

    PlayerAccessPoint playerAccessPoint;
    private void Start() 
    {
        _originalPosition = _currentTransform.position;
        _originalRotation = _currentTransform.rotation;
        playerAccessPoint = FindObjectOfType<PlayerAccessPoint>(); //.. find the PlayerAccessPoint object
    }

    private void Update() 
    {
        ManagePosition();
        ManageRotation();
    }
    private bool IsNear() { return Vector3.Distance(transform.position, _targetPosition) < m_nearDistance; }
    
    public void Interact(Vector3 Sendposition)
    {
        _targetPosition = Sendposition;
        _canInspect = !_canInspect;
        ManagePlayerAction();
    }

    #region Main
        private void ManagePosition()
        {
            Vector3 tp = _canInspect ? _targetPosition : _originalPosition;
            transform.position = Vector3.Lerp(transform.position, tp, m_lerpSpeed * Time.deltaTime);
        }
        private void ManageRotation()
        {
            if (_canInspect && Input.GetMouseButton(0) && IsNear())
            {
                float YaxisRotation = Input.GetAxis("Mouse X") * m_rotationSpeed;
                float XaxisRotation = Input.GetAxis("Mouse Y") * m_rotationSpeed;

                transform.Rotate(Vector3.up, YaxisRotation);
                transform.Rotate(Vector3.forward, XaxisRotation);
            }
            else if (!IsNear())
            {
                Quaternion a = transform.rotation;
                transform.rotation = Quaternion.Lerp(a, _originalRotation, m_lerpSpeed * Time.deltaTime);
            }
        }
        private void ManagePlayerAction()
        {
            if (playerAccessPoint == null) { return; }

            if (_canInspect && !_alreadyCalledPAP)
            {
                _alreadyCalledPAP = true;
                playerAccessPoint.sendRequest = PlayerAccessPoint.SendRequest.DoInteraction;
            }
            else if (!_canInspect && _alreadyCalledPAP)
            {
                _alreadyCalledPAP = false;
                playerAccessPoint.sendRequest = PlayerAccessPoint.SendRequest.DoMovement;
            }
        }
    #endregion

}