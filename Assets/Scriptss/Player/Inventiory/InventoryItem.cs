using UnityEngine;


[CreateAssetMenu(fileName = "InventoryItem", menuName = "EXOS LIGHT AV1/InventoryItem", order = 0)]
public class InventoryItem : ScriptableObject 
{
    public string ItemName;
    public string ItemType;
    public string ItemUseable;
    public int ItemWeight = 20;
    public Sprite ItemSprite; 
}
