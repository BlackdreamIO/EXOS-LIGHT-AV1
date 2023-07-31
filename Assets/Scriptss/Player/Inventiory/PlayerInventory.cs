using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    private void Start() 
    {
        if(Instance == null) { Instance = this; }
        else {  Destroy(gameObject); }
    }
    public void AddItem(InventoryItem scirptableData)
    {
        Debug.Log($"Name : {scirptableData.ItemName} | Item Type : {scirptableData.ItemType} | Item Useable : {scirptableData.Useable}");
    }
    public void UseItem()
    {
        
    }
    public void RemoveItem()
    {

    }
}
