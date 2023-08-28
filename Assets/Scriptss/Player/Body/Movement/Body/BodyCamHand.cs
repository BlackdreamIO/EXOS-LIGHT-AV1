using UnityEngine;
using EL.Core.Player;

public class BodyCamHand : MonoBehaviour
{
    [SerializeField] Player playerScript;
    [SerializeField] Transform m_camera;

    [SerializeField] float currentFrequency = 15;
    [SerializeField] float currentAmount = 2;

    private void Update()
    {
        float playerMagnitude = new Vector3(playerScript.playerInputX, 0, playerScript.playerInputY).magnitude;

        // transform.Rotate(0, m_camera.transform.rotation.y, 0);
        transform.localRotation = m_camera.transform.rotation;

        if (playerMagnitude > 0)
        {
            StartBOB();
        }
    }
    private Vector3 StartBOB() // Hand Bob Effect For Walking Or Running
    {
        Vector3 pos = Vector3.zero;
        pos.y = Mathf.Sin(Time.time * currentFrequency) * currentAmount * 1.4f;
        pos.x = Mathf.Sin(Time.time * currentFrequency / 2f) * currentAmount * 1.6f;

        transform.localPosition = pos;
        return pos;
    }
}
