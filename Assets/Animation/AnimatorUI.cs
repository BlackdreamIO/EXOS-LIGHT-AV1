using UnityEngine;
using UnityEngine.UI;
public class AnimatorUI : MonoBehaviour
{   [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector3 vector3From;
    [SerializeField] private Vector3 vector3To;
    [SerializeField] private float Speed;
    [SerializeField] private bool UseCurrentPositionAsFrom = true;
    public bool StartTransistion;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (UseCurrentPositionAsFrom)
        {
            vector3From = this.transform.position;
        }
    }
    private void Update()
    {
        if(StartTransistion)
        {
            rectTransform.position = Vector3.Lerp(vector3From, vector3To, Speed * Time.deltaTime);
        }
        else
        {
            rectTransform.position = Vector3.Lerp(vector3To, vector3From, Speed * Time.deltaTime);
        }
    }
}
