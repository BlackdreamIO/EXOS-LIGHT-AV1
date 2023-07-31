using UnityEngine;

interface IInteractable
{
    public void Interact();
}
interface Iinspectable
{
    public void Inspect(Transform objectHolder);
}

interface IpickUpItem
{
    public void PickUpItem();
}
public class interactor : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform objectInspectTransform;
    [SerializeField] KeyCode InteractKey = KeyCode.E;
    [SerializeField] KeyCode InspectKey = KeyCode.E; 
    [SerializeField] KeyCode PickUpItemkey = KeyCode.E;
    [SerializeField] float range = 10f;

    RaycastHit hit;

    private void Update()
    {
        CheckInspectorMethod();
        CheckInteractMethod();
        CheckFoundInventoryItem();
    }

    private void CheckInspectorMethod()
    {
        if (Input.GetKeyDown(InspectKey)) //, Inspector Caller
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
            {
                if (hit.collider.TryGetComponent(out Iinspectable InspectObj))
                {
                    InspectObj.Inspect(objectInspectTransform);
                }
            }
        }
    }

    private void CheckInteractMethod()
    {
        if (Input.GetKeyDown(InteractKey)) //, Ineract Caller
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
            {
                if (hit.collider.TryGetComponent(out IInteractable InteractObj))
                {
                    InteractObj.Interact();
                }
            }
        }
    }
    private void CheckFoundInventoryItem()
    {
        if (Input.GetKeyDown(PickUpItemkey)) //, Ineract Caller
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
            {
                if (hit.collider.GetComponent<InventoryItemObject>())
                {
                    if (hit.collider.TryGetComponent(out IpickUpItem IpickUpItem))
                    {
                        IpickUpItem.PickUpItem();
                    }
                }
            }
        }
    }   
}

