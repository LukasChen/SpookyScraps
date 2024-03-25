using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName="InventoryEvent",menuName="Events/Inventory Data Event Channel")]
public class InventoryDataEventChannelSO : ScriptableObject {
    public UnityAction<InventoryItemData> OnEventRaised;

    public void RaiseEvent(InventoryItemData data) {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(data);
    }
}