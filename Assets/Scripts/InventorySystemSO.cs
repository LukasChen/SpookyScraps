using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Inventory System")]
public class InventorySystemSO : ScriptableObject {
    public List<InventoryItemData> InventoryItems { get; private set; } = new();

    public int InventoryCount => InventoryItems.Count;

    public InventoryItemData CurrentItem => InventoryItems[SelectedIndex];

    public int SelectedIndex { get; set; } = 0;

    public UnityAction<InventoryItemData> OnItemAdd;
    public UnityAction<int> OnItemRemove;
    
    public void AddItem(InventoryItemData item) {
        InventoryItems.Add(item);
        SelectedIndex = InventoryCount - 1;
        OnItemAdd.Invoke(item);
    }

    public void RemoveItem(int index) {
        InventoryItems.RemoveAt(index);
        if (OnItemRemove != null) OnItemRemove.Invoke(index);
    }

    public void Clear() {
        SelectedIndex = 0;
        InventoryItems.Clear();
    }
    
    public void OnBeforeSerialize() {
        InventoryItems.Clear();
    }

    public void OnAfterDeserialize() {
        InventoryItems.Clear();
    }
}