using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public List<InventoryItemData> InventoryItems { get; private set; } = new();
    [SerializeField] private InventoryDataEventChannelSO _updateInventory;
    public void AddItem(InventoryItemData item) {
        InventoryItems.Add(item);
        _updateInventory.RaiseEvent(item);
    }
}