﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Inventory System")]
public class InventorySystem : ScriptableObject {
    public List<InventoryItemData> InventoryItems { get; private set; } = new();
    
    public UnityAction<InventoryItemData> OnItemAdd;
    public UnityAction<InventoryItemData> OnItemRemove;
    
    public void AddItem(InventoryItemData item) {
        InventoryItems.Add(item);
        OnItemAdd.Invoke(item);
    }

    public void RemoveItem(InventoryItemData item) {
        InventoryItems.Remove(item);
        OnItemRemove.Invoke(item);
    }
}