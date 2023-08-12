using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EL.Core.Player;
using TetraCreations.Attributes;

public class PlayerUIManager : MonoBehaviour
{   
    public Player player;

    #region Player Side
    
    [SerializeField] private Slider StaminaSlider;

    #endregion

    #region Inventory Side
    [SerializeField] bool ShowInventoryUI;
    [DrawIf(nameof(ShowInventoryUI),true, DisablingType.DontDraw)]  [SerializeField] private GameObject InventoryItemHolder;
    [DrawIf(nameof(ShowInventoryUI), true, DisablingType.DontDraw)] [SerializeField] private GameObject InventoryItemPrefab;
    [DrawIf(nameof(ShowInventoryUI), true, DisablingType.DontDraw)] [SerializeField] private TextMeshProUGUI inventoryItemName;
    [DrawIf(nameof(ShowInventoryUI), true, DisablingType.DontDraw)] [SerializeField] private Sprite inventoryItemSprite;
    #endregion
    private void Start()
    {
        if(player != null) 
        {
            StaminaSlider.minValue = player.m_minStamina;
            StaminaSlider.maxValue = player.m_Stamina;
        }
    }
    private void Update() 
    {
        StaminaSlider.value = player.m_currentStamina;
    }

    public void PopUpObjectID(string id)
    {
        
    }

    /*
    public System.Collections.Generic.List<GameObject> gameObjects;

    [ContextMenu("CreateInventoryItem")]
    public void CreateInventoryItem(string text)
    {
        for (int i = 0; i < InventoryItemHolder.transform.childCount; i++)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.GetComponentInChildren<TextMeshProUGUI>().text == (gameObject.GetComponentInChildren<TextMeshProUGUI>().text))
                {
                    Debug.Log("DuplicateDetected");
                }
            }   
        }
        var inventoryItem = GameObject.Instantiate(InventoryItemPrefab, InventoryItemHolder.transform);
        TextMeshProUGUI itemText = inventoryItem.GetComponentInChildren<TextMeshProUGUI>();

        if (inventoryItem.GetComponentInChildren<TextMeshProUGUI>())
        {
            itemText.text = text;
        }

        if(gameObjects.Count < InventoryItemHolder.transform.childCount)
        {
            gameObjects.Add(inventoryItem);
        }

        Image itemImage = inventoryItem.GetComponentInChildren<Image>();
        if (itemImage != null)
        {
            itemImage.sprite = inventoryItemSprite;
        }
    }
    */

}
