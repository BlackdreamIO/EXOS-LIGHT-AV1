using UnityEngine;
using EL.Player;
public class Interactor : MonoBehaviour
{
    [SerializeField] private float m_Intensity = 5f;
    [SerializeField] private float m_Amount = 1f;
    [SerializeField] private bool m_InvertX, m_InvertY;
    public GameObject targetObject;
    void Update()
    {
        //, Get InputAxis
        float movementX = Input.GetAxis("Horizontal") * m_Intensity;
        float movementY = Input.GetAxis("Vertical") * m_Intensity;

        //, Toggle Positive Negetive Value From Boolean
        float outputY = m_InvertY ? -movementY : movementY;
        float outputX = m_InvertX ? -movementX : movementX;

        //, Handle The Rotation Of Target Object
        Quaternion a = Quaternion.identity;
        Quaternion b = Quaternion.Euler(outputY, 0, outputX);
        targetObject.transform.localRotation = Quaternion.Slerp(a, b, m_Amount);

    }
}
