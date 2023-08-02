using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [SerializeField] private int Capacity = 50;
    public List<InventoryItem> inventoryItems;


    //, Scirpts Variables
    private int currentCapacity;

    private void Start() 
    {
        if(Instance == null) { Instance = this; } //, create singleton
        else {  Destroy(gameObject); }

        currentCapacity = Capacity;
    }
    public void AddItem(InventoryItem scirptableData)
    {
        if(CalculateCapacity(scirptableData.ItemWeight, currentCapacity, Capacity))
        {
            inventoryItems.Add(scirptableData);
            CheckForDuplicateItem();
            currentCapacity -= scirptableData.ItemWeight;
            Debug.Log($"Name : {scirptableData.ItemName} | Item Type : {scirptableData.ItemType} | Item Useable : {scirptableData.ItemUseable}");
            Debug.Log("CurrentCapacity :: " + currentCapacity);
        }
        else
        {
            Debug.Log("Inventory is full");
        }

        bool CalculateCapacity(int ItemWeight, int InventoryCapacity, int InventoryMaxCapacity)
        {
            if( ItemWeight - InventoryCapacity < InventoryMaxCapacity && InventoryCapacity > 1
                || ItemWeight - InventoryCapacity == InventoryMaxCapacity && InventoryCapacity > 1 )
            {
                return true;
            }
            return false;
        }

    }

    [ContextMenu("UseItem")]
    public void UseItem()
    {
        // code for useItem
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            Debug.Log(inventoryItems[i].ItemName
            + " | " + inventoryItems[i].ItemType
            + " | " + inventoryItems[i].ItemUseable
            + " | " + inventoryItems[i].ItemWeight);
        }
    }
    public void RemoveItem()
    {
        // remove the item
    }

    private void CheckForDuplicateItem()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            for (int j = i + 1; j < inventoryItems.Count; j++)
            {
                if (inventoryItems[i].ItemName == inventoryItems[j].ItemName)
                {
                    //inventoryItems.RemoveAt(j);
                    //j--;
                    Debug.Log("Added Duplicated Item To Inventory 🖥");
                }
            }
        }
    }
}
