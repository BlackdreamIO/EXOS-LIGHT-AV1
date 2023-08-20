using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
public class Interactor : MonoBehaviour
{   
    [SerializeField] PlayerUIManager playerUIManager;
    [SerializeField] DepthOfFieldDeprecated depthOfFieldDeprecated; 
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform objectInspectTransform;
    [SerializeField] KeyCode InteractKey = KeyCode.E;
    public Light IneractLight;
    [SerializeField] float range = 10f;

    RaycastHit hit;

    public List<InspectorObject> inspectorsList;
    private void Start() 
    {
        InspectorObject[] interactableObjects = FindObjectsOfType<InspectorObject>();

        for (int i = 0; i < interactableObjects.Length; i++)
        {
            inspectorsList.Add(interactableObjects[i]);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(InteractKey))
        {
            CheckForInteractDetection();
        }
    }

    private void CheckForInteractDetection()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            if (hit.collider.TryGetComponent(out InspectorObject inspectorObject))
            {
                Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward, Color.cyan, range);
                inspectorObject.Inspect(objectInspectTransform);
            }
            if(hit.collider.TryGetComponent(out InventoryItemObject inventoryItemObject))
            {
                PlayerInventory.Instance.AddItem(inventoryItemObject.itemData);
            }
        }
    }
}

