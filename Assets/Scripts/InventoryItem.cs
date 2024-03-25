using UnityEngine;

public class InventoryItem : MonoBehaviour {
    [SerializeField] private InventoryItemData _data;

    public void HandlePickup(InventoryManager _manager) {
        Debug.Log(_manager);
        _manager.AddItem(_data);
        Destroy(gameObject);
    }
}