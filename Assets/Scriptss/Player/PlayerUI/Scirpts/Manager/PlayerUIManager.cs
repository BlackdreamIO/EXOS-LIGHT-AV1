using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    public Slider UI_staminaSlider;
    public Slider UI_flashLightChargeSlider;


    private void Start()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    public GameObject InventoryItemHolder;
    public GameObject InventoryItemPrefab;
    public TextMeshProUGUI inventoryItemName;
    public Sprite inventoryItemSprite;

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

}
