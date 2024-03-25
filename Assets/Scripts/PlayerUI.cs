using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUI : MonoBehaviour {
    [SerializeField] private InventoryDataEventChannelSO _addInventory;
    
    private VisualElement _root;
    private VisualElement _inventory;

    private void OnEnable() {
        _addInventory.OnEventRaised += AddItem;
    }

    private void Start() {
        _root = GetComponent<UIDocument>().rootVisualElement;
        
        // bind inventory
        _inventory = _root.Q<VisualElement>("inventory-row");
        Debug.Log(_inventory);
    }

    private void AddItem(InventoryItemData item) {
        _inventory.Add(BuildItem(item));
    }

    private VisualElement BuildItem(InventoryItemData itemData) {
         VisualElement item = new VisualElement();
         item.AddToClassList("inventory-item");
         Label itemName = new Label(itemData.name);
         itemName.AddToClassList("inventory-text");
         Image itemImg = new Image();
         itemImg.sprite = itemData.Icon;
         
         item.Add(itemImg);
         item.Add(itemName);
         return item;
    }
}