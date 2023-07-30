using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OulineBlend : MonoBehaviour
{
    [SerializeField] bool EnableBlend = true;
    [SerializeField] float BlendSpeed = 3f;
    [SerializeField] float BlendDistance = 3f;
    [SerializeField] Outline outline => GetComponent<Outline>();
    [SerializeField] string playerTag;

    [SerializeField] enum TargetObject { Player, Specific }
    [SerializeField] TargetObject targetObjectType;

    float originalOutlineWidth;
    float currentOutlineWidth;
    private GameObject playerObject;

    private void Start() 
    {
        originalOutlineWidth = outline.OutlineWidth; 
        currentOutlineWidth = originalOutlineWidth;
    }
    void Update()
    {
        playerObject = GameObject.FindGameObjectWithTag(playerTag);

        if(playerObject == null) { return; }

        float playerDistance = Vector3.Distance(playerObject.transform.position,  this.transform.position);


        if(playerDistance < BlendDistance) 
        {
            outline.OutlineWidth = currentOutlineWidth;
            currentOutlineWidth += BlendSpeed * Time.deltaTime;
            currentOutlineWidth = Mathf.Clamp(currentOutlineWidth, 0, originalOutlineWidth);
        }

        else if(playerDistance > BlendDistance) 
        {
            outline.OutlineWidth = currentOutlineWidth;
            currentOutlineWidth -= BlendSpeed * Time.deltaTime;
            currentOutlineWidth = Mathf.Clamp(currentOutlineWidth, 0, originalOutlineWidth);
        }

    }
}
