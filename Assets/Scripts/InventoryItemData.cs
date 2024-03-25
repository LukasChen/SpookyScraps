using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Item", menuName = "Inventory/Inventory Item", order = 0)]
public class InventoryItemData : ScriptableObject {
    public string id;
    public string name;
    public Sprite Icon;
    public GameObject prefab;
}