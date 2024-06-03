using UnityEngine;

public class AwarenessConeAlignment : MonoBehaviour {
    [SerializeField] private LayerMask _mask;
    private void Update() {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.5f, ~_mask)) {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            transform.rotation = rotation;
        }
    }
}