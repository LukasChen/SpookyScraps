using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerUI : MonoBehaviour {
    [SerializeField] private InventorySystemSO _inventoryManager;
    
    private VisualElement _root;
    private VisualElement _inventory;

    private void OnEnable() {
        _inventoryManager.OnItemAdd += AddItem;
    }

    private void Start() {
        _root = GetComponent<UIDocument>().rootVisualElement;
        
        // bind inventory
        _inventory = _root.Q<VisualElement>("inventory-row");
        Debug.Log(_inventory);
    }

    private void AddItem(InventoryItemData item) {
        VisualElement itemElement = BuildItem(item);
        if (_inventoryManager.InventoryItems.Count == 1) {
        }
        
        itemElement.AddToClassList("active");
        Debug.Log(_inventoryManager.InventoryItems.Count);
        _inventory.Add(itemElement);
    }

    public void OnScrollWheel(InputAction.CallbackContext ctx) {
        Debug.Log(ctx.ReadValue<Vector2>());
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