using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNormalState : PlayerBaseState {

    public PlayerNormalState(PlayerControl player) : base(player) { }
    
    public override void UpdateState() {
        HandleMovement();
    }

    public override void EnterState() {
        player.Inputs.Player.Aim.started += SwitchToAimState;
        player.Inputs.Player.Interact.performed += OnInteract;
        player.Inputs.Player.Drop.performed += OnDrop;
    }

    private void SwitchToAimState(InputAction.CallbackContext obj) {
        player.SwitchState(player.AimState);
    }

    public override void ExitState() {
        player.Inputs.Player.Aim.started -= SwitchToAimState;
        player.Inputs.Player.Interact.performed -= OnInteract;
        player.Inputs.Player.Drop.performed -= OnDrop;
    }

    private void HandleMovement() {
        bool isRunning = player.Inputs.Player.Sprint.ReadValue<float>() == 1;

        float acceleration = isRunning ? player.runAcceleration : player.acceleration;
        float maxSpeed = isRunning ? player.runSpeed : player.speed;

        player.SimpleMove(maxSpeed, acceleration);
        
        if (player.HasInputThisFrame) {
            player.LookDir(player.CurrentInput);
        }
        
        float animatorVel = player.LocalVelocity.z / player.runSpeed;
        player.Animator.SetFloat("velocityZ", animatorVel);
    }
    
    private void OnInteract(InputAction.CallbackContext ctx) {
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, 0.5f, ~player.playerLayer);
        foreach (var hit in hitColliders) {
            if (hit.transform.TryGetComponent<InventoryItem>(out InventoryItem item)) {
                item.HandlePickup(player.inventory);
                break;
            }
        }
    }
    
    private void OnDrop(InputAction.CallbackContext obj) {
        // TODO: Check if terrain good for dropping

        if (player.inventory.InventoryCount == 0) return;

        var discardItem = player.inventory.CurrentItem;
        Vector3 dropPos = new Vector3(Random.Range(player.transform.position.x - 0.5f, player.transform.position.x + 0.5f),
            player.transform.position.y, Random.Range(player.transform.position.z - 0.5f, player.transform.position.z + 0.5f));
        Object.Instantiate(discardItem.prefab, dropPos, Quaternion.identity);
        player.inventory.RemoveItem(player.inventory.SelectedIndex);
    }

}