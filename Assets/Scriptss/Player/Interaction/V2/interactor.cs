using UnityEngine;

interface IInteractable
{
    public void Interact();
}
interface Iinspectable
{
    public void Inspect(Transform objectHolder);
}

public class interactor : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform objectInspectTransform;
    [SerializeField] KeyCode InteractKey = KeyCode.E;
    [SerializeField] KeyCode InspectKey = KeyCode.E; 
    [SerializeField] float range = 10f;

    private void Update() 
    {
        RaycastHit hit;

        if(Input.GetKeyDown(InteractKey)) 
        {
            if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit , range)){
                
                if(hit.collider.TryGetComponent(out IInteractable InteractObj)) 
                {
                    InteractObj.Interact();
                }
            }
        }

        if(Input.GetKeyDown(InteractKey)) 
        {
            if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit , range)){
                
                if(hit.collider.TryGetComponent(out Iinspectable InspectObj)) 
                {
                    InspectObj.Inspect(objectInspectTransform);
                }
            }
        }

    }
}
