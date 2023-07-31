using UnityEngine;

public class InventoryItemObject : MonoBehaviour, IpickUpItem
{
    public InventoryItem itemData;

    public void PickUpItem()
    {
        PlayerInventory.Instance.AddItem(itemData);
    }
}
