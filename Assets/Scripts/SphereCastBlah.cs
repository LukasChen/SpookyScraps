using UnityEngine;

public class SphereCastBlah : MonoBehaviour {
    private void Update() {
        int mask = LayerMask.GetMask("Player");
        // RaycastHit[] hits = Physics.SphereCastAll(transform.position,0.5f, Vector3.forward, 10, ~mask);
        // foreach (var hit in hits) {
        //     Debug.Log(hit.collider);
        // }
        if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out var hit, 10)) {
            Debug.Log(hit.collider);
            
        }
    }
}