using UnityEngine;

public class InventoryItem : MonoBehaviour {
    [SerializeField] private InventoryItemData _data;

    public void HandlePickup(InventorySystemSO _manager) {
        _manager.AddItem(_data);
        Destroy(gameObject);
    }
}