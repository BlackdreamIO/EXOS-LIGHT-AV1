using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interactor : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] float objectGrabSpeed = 5f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform originTransform;
    [SerializeField] Transform InteractableHolderTransform;
    [SerializeField] KeyCode InspectKey;
    [SerializeField] KeyCode InteractKey;
    [SerializeField] KeyCode GrabKey;
    [SerializeField] string ObjectName;


    [SerializeField] float rotationSpeed = 5f;
    private bool isRotating = false;
    bool canInteract = true;
    bool carring;
    bool toggleInspact;
    bool isInspected;
    bool foundInteractableObject;
    GameObject inspecObject;
    GameObject InteractableObject;
    Vector3 oldInspectObjectPosition;
    Vector3 currentInspectObjectGrabPosition;
    Quaternion oldInspectObjectRotation;
    Quaternion  currentInspectObjectRotation;
    Transform oldParent;
    Vector3 PrevPos = Vector3.zero;
    Vector3 PosDelta = Vector3.zero;
    Vector3 Rotation;

    /*
    private void Update() 
    {
        //Rotation = new Vector3(Player.instance.playerCamera.transform.eulerAngles.x, 0f,0f);
        //InteractableHolderTransform.localRotation = Quaternion.Euler(Rotation);
        //Debug.Log(Player.instance.playerCamera.transform.eulerAngles.x);

        if (Input.GetKeyDown(InspectKey))
        {            
            toggleInspact =! toggleInspact;

            if(toggleInspact && canInspect)
            {
                INSPECT_OBJECT();
                RESET_INSPECTED_OBJECT_ROTATION();
                Player.instance.DisableMouseMovement();
                canInspect = false;
                isInspected = false;
            }
            else if(!toggleInspact && isInspected)
            {
                startCoroutine();
            }
            carring = true;
        }

        if(Input.GetKeyDown(InteractKey) && foundInteractableObject) 
        {
            //canInteract = InteractableObject.GetComponent<InteractableScript>().canInteractWithDoor();

            if(canInteract && InteractableObject != null)
            {
                //InteractableObject.GetComponent<IinspectorObject>().Interact();
            }

        }

       
    }
    */

     /*
    GameObject GetHitObject()
    {
        RaycastHit hit;
        if(Physics.Raycast(originTransform.position, originTransform.forward, out hit, range, layerMask))
        {
            if(hit.collider.GetComponent<InspectorObject>())
            {

                inspecObject = hit.collider.GetComponent<InspectorObject>().gameObject;

                oldInspectObjectPosition = inspecObject.transform.position;
                oldInspectObjectRotation = hit.transform.localRotation;

                if(hit.transform.parent){ oldParent = hit.transform.parent; }

                return inspecObject;
            }

            foundInteractableObject = hit.collider.GetComponent<InteractableScript>();
            InteractableObject = hit.collider.GetComponent<InteractableScript>().gameObject;
            canInteract = InteractableObject.GetComponent<InteractableScript>().canInteractWithDoor();
        }
        return null;
    } 
    
    private void INSPECT_OBJECT()
    {
        
        if(inspecObject == null)
        {
            inspecObject = GetHitObject();

            var obj = inspecObject.GetComponent<InspectorObject>();

            StartCoroutine(MOVE_OBJECT_TO_ARM(obj)); 
            ObjectName = inspecObject.GetComponent<InspectorObject>().GetObjectName();
            RESET_INSPECTED_OBJECT_ROTATION();
        }
    }


    private IEnumerator MOVE_OBJECT_TO_ARM(InspectorObject obj)
    {
        while (obj.transform.position != InteractableHolderTransform.position)
        {
            obj.GrabObjectToArm(InteractableHolderTransform.position, objectGrabSpeed);
            RESET_INSPECTED_OBJECT_ROTATION();
            yield return null;
        }

        obj.transform.SetParent(InteractableHolderTransform);
        currentInspectObjectGrabPosition = obj.transform.position;
        currentInspectObjectRotation = obj.transform.localRotation;
        canInspect = false;
        isInspected = true;
    }
    public void startCoroutine()
    {
        var obj = inspecObject.GetComponent<InspectorObject>();
        StartCoroutine(MoveObjectToDestination(obj));
    }
    public IEnumerator MoveObjectToDestination(InspectorObject obj)
    {
        while (obj.transform.position != oldInspectObjectPosition)
        {   
            if(inspecObject.transform.parent)
            {
                inspecObject.transform.SetParent(oldParent);
            }
            inspecObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            inspecObject.transform.localRotation = Quaternion.identity;

            obj.GrabObjectToArm(oldInspectObjectPosition, objectGrabSpeed);
            yield return null;
        }
        currentInspectObjectGrabPosition = obj.transform.position;
        currentInspectObjectRotation = obj.transform.localRotation;
        inspecObject = null;
        canInspect = true;
        Player.instance.EnableMouseMovement();
        ObjectName = "";
        carring = false;
    }
*/
    private void RESET_INSPECTED_OBJECT_ROTATION()
    {
        if(inspecObject != null)
        {
            if (inspecObject.transform.parent == InteractableHolderTransform)
            {
                inspecObject.transform.position = InteractableHolderTransform.position;
                inspecObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                inspecObject.transform.localRotation = Quaternion.identity;
            }
        }
    }
    private void HandleObjectRotation()
    {
        if (Input.GetMouseButton(0) && carring)
        {
            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

            inspecObject.transform.Rotate(Vector3.up, XaxisRotation);
            inspecObject.transform.Rotate(Vector3.forward, YaxisRotation);
        }
    }
}
