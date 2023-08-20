using UnityEngine;

public class InventoryItemObject : MonoBehaviour
{
    public InventoryItem itemData;

    public void PickUpItem()
    {
        PlayerInventory.Instance.AddItem(itemData);
    }
}
