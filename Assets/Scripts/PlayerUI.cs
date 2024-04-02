using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerUI : MonoBehaviour {
    [SerializeField] private InventorySystemSO _inventoryManager;
    [SerializeField] private Sprite _activeBox;
    [SerializeField] private Color _activeColor;
    
    private VisualElement _root;
    private VisualElement _inventory;
    

    private void OnEnable() {
        _inventoryManager.OnItemAdd += AddItem;
        _inventoryManager.OnItemRemove += RemoveItem;
    }

    

    private void Start() {
        _root = GetComponent<UIDocument>().rootVisualElement;
        
        // bind inventory
        _inventory = _root.Q<VisualElement>("inventory-row");
        Debug.Log(_inventory);
    }

    private void AddItem(InventoryItemData item) {
        VisualElement itemElement = BuildItem(item);
        
        Debug.Log(_inventoryManager.InventoryItems.Count);
        _inventory.Add(itemElement);
        SetItemActive();
    }
    
    private void RemoveItem(int index) {
        _inventory.Query<VisualElement>("inventory-item").AtIndex(index).RemoveFromHierarchy();
    }

    public void OnScrollWheel(InputAction.CallbackContext ctx) {
        float wheel = ctx.ReadValue<Vector2>().y;
        if (wheel == 0 || _inventoryManager.InventoryCount == 0) return;
        if (wheel > 0) {
            _inventoryManager.SelectedIndex = (_inventoryManager.SelectedIndex + 1) % _inventoryManager.InventoryCount;
        }
        else {
            _inventoryManager.SelectedIndex = (_inventoryManager.SelectedIndex + _inventoryManager.InventoryCount - 1) % _inventoryManager.InventoryCount;
        }

        SetItemActive();
    }

    private void SetItemActive() {
        int i = 0;
        foreach (var item in _inventory.Children()) {
            if (i == _inventoryManager.SelectedIndex) {
                item.AddToClassList("active");
                item.Q<Image>("inventory-select").tintColor = _activeColor;
            }
            else {
                item.RemoveFromClassList("active");
                item.Q<Image>("inventory-select").tintColor = Color.white;
            }
            i++;
        }
    }

    private VisualElement BuildItem(InventoryItemData itemData) {
         VisualElement item = new VisualElement();
         item.AddToClassList("inventory-item");
         item.name = "inventory-item";
         Label itemName = new Label(itemData.name);
         itemName.AddToClassList("inventory-text");
         
         VisualElement imgContainer = new VisualElement();
         imgContainer.AddToClassList("inventory-imgwrapper");
         Image itemImg = new Image();
         itemImg.sprite = itemData.Icon;
         itemImg.AddToClassList("inventory-img");
         Image active = new Image();
         active.sprite = _activeBox;
         active.AddToClassList("inventory-select");
         active.name = "inventory-select";
         
         imgContainer.Add(active);
         imgContainer.Add(itemImg);
         item.Add(imgContainer);
         item.Add(itemName);
         return item;
    }
}