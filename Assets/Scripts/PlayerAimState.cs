using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAimState : PlayerBaseState {

    private IEnumerator _aimCoroutine;
    
    public PlayerAimState(PlayerControl player) : base(player) { }

    public override void UpdateState() {
        var (success, mouseWorldPos) = GetWorldMousePos();
        if (success) {
            Vector3 targetDir = mouseWorldPos - player.transform.position;
            player.LookDir(new Vector2(targetDir.x, targetDir.z));
        }
        
        
        player.SimpleMove(player.speed, player.acceleration);

        float animatorXVel = player.LocalVelocity.x / player.speed;
        float animatorZVel = player.LocalVelocity.z / player.speed;
        player.Animator.SetFloat("velocityX", animatorXVel);
        player.Animator.SetFloat("velocityZ", animatorZVel);
    }

    public override void EnterState() {
        player.Inputs.Player.Aim.canceled += SwitchToNormal;
        
        player.ToggleLaser.RaiseEvent(true);

        _aimCoroutine = AnimateAim(1, 0.25f);
        player.StartCoroutine(_aimCoroutine);
    }


    public override void ExitState() {
        player.Inputs.Player.Aim.canceled -= SwitchToNormal;
        
        player.ToggleLaser.RaiseEvent(false);
        
        player.StopCoroutine(_aimCoroutine);
        _aimCoroutine = AnimateAim(0, 0.25f);
        player.StartCoroutine(_aimCoroutine);
    }
    
    
    private void SwitchToNormal(InputAction.CallbackContext obj) {
        player.SwitchState(player.NormalState);
    }
    
    private (bool, Vector3) GetWorldMousePos() {
        Ray ray = player.cam.ScreenPointToRay(player.Inputs.Player.Look.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hit, 20, player.groundLayer)) {
            return (true, hit.point);
        } else {
            return (false, Vector3.zero);
        }
    }
    
    private IEnumerator AnimateAim(float targetWeight, float duration) {
        float original = player.Animator.GetLayerWeight(1);
        yield return CoroutineUtils.Lerp(duration, t => {
            player.Animator.SetLayerWeight(1, Mathf.Lerp(original, targetWeight, t));
        });
    }
}