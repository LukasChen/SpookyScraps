using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerControl : MonoBehaviour {
    [FormerlySerializedAs("_speed")] [SerializeField] public float speed;
    [FormerlySerializedAs("_runSpeed")] [SerializeField] public float runSpeed;
    [FormerlySerializedAs("_acceleration")] [SerializeField] public float acceleration = 0.5f;
    [FormerlySerializedAs("_runAcceleration")] [SerializeField] public float runAcceleration = 1;
    [FormerlySerializedAs("_turnSmoothTime")] [SerializeField] public float turnSmoothTime;
    [FormerlySerializedAs("_playerLayer")] [SerializeField] public LayerMask playerLayer;
    [FormerlySerializedAs("_groundLayer")] [SerializeField] public LayerMask groundLayer;
    [FormerlySerializedAs("_cam")] [SerializeField] public Camera cam;
    [SerializeField] private float _inputSmoothSpeed;
    
    [FormerlySerializedAs("_inventory")] [SerializeField] public InventorySystemSO inventory; 
    
    public PlayerInputActions Inputs { get; private set; }
    public CharacterController Controller { get; private set; }
    public Animator Animator { get; private set; }


    public float Velocity { get; private set; }
    public Vector3 LocalVelocity => Quaternion.Euler(0, -CurrentAngle, 0) * MoveVelocity;
    private float _turnSmoothVelocity;
    private Vector2 _inputSmoothVelocity;
    public Vector2 CurrentInput { get; private set; }
    public Vector3 MoveVelocity { get; private set; }
    
    public float CurrentAngle { get; private set; }

    // State
    public PlayerBaseState CurrentState;
    public PlayerNormalState normalState;
    public PlayerAimState aimState;

    public void SwitchState(PlayerBaseState newState) {
        CurrentState?.ExitState();
        CurrentState = newState;
        newState.EnterState();
    }

    private void OnEnable() {
       Inputs = new PlayerInputActions();
       Inputs.Enable();
       
       // Inputs
       // Inputs.Player.Interact.performed += OnInteract;
       // Inputs.Player.Drop.performed += OnDrop;
       // Inputs.Player.Aim.started += OnAim;
       // Inputs.Player.Aim.canceled += OnAim;
       inventory.Clear();
    }
    

    private void OnDisable() {
       Inputs.Disable();
       // Inputs.Player.Interact.performed -= OnInteract;
       // Inputs.Player.Drop.performed -= OnDrop;
       // Inputs.Player.Aim.started -= OnAim;
       // Inputs.Player.Aim.canceled -= OnAim;
    }
    private void Start() {
        Controller = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();

        normalState = new PlayerNormalState(this);
        aimState = new PlayerAimState(this);
        SwitchState(normalState);
    }

    private void Update() {
        // Vector2 movementInput = movementAction.ReadValue<Vector2>();
        // Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        // bool isRunning = Inputs.Player.Sprint.ReadValue<float>() == 1;
        //
        // float acceleration = isRunning ? runAcceleration : this.acceleration;
        // float maxSpeed = isRunning ? runSpeed : speed;
        
        // if (movementInput.magnitude >= 0.1f) {
        //     if (!isAiming) 
        //         LookDir(new Vector2(movementInput.x, movement.z));
        //     
        //     if (velocity > speed && !isRunning) {
        //         velocity = Mathf.Clamp(velocity - acceleration * Time.deltaTime,0, runSpeed);
        //     }
        //     else {
        //         velocity = Mathf.Clamp(velocity + acceleration * 2 * Time.deltaTime, 0, maxSpeed);
        //     }
        // }
        // else {
        //     velocity = Mathf.Clamp(velocity - acceleration * 2 * Time.deltaTime,0, runSpeed);
        // }
        
        CurrentState.UpdateState();

        // if (isAiming) {
        //     var (success, mouseWorldPos) = GetWorldMousePos();
        //     if (success) {
        //         Vector3 targetDir = mouseWorldPos - transform.position;
        //         Debug.Log(mouseWorldPos);
        //         LookDir(new Vector2(targetDir.x, targetDir.z));
        //     }
        // }
        
        // float animatorVel = velocity / runSpeed;
        // animator.SetFloat("velocityZ", animatorVel);
        //
        // Vector3 move = new Vector3(movement.x, Physics.gravity.y, movement.z);
        // controller.Move(move * (velocity * Time.deltaTime));
    }

    public void ReadMovementInput() {
        Vector2 movementInput = Inputs.Player.Move.ReadValue<Vector2>();
        Vector2 movement = movementInput.normalized;
        CurrentInput = Vector2.SmoothDamp(CurrentInput, movement, ref _inputSmoothVelocity, _inputSmoothSpeed);
    }

    public void CalculateMoveVelocity(bool hasMovementInput,float currentMaxSpeed, float acceleration) {
        if (hasMovementInput && Velocity < currentMaxSpeed) {
           Velocity = Mathf.Clamp(Velocity + acceleration * Time.deltaTime, 0, currentMaxSpeed);
        }
        else {
           Velocity = Mathf.Clamp(Velocity - acceleration  * 2 * Time.deltaTime,0, runSpeed);
        }
    }

    public void Move() {
        MoveVelocity = new Vector3(CurrentInput.x * Velocity, Physics.gravity.y, CurrentInput.y * Velocity);
        Controller.Move(MoveVelocity * Time.deltaTime);
    }


    public void LookDir(Vector2 direction) {
         float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
         // Debug.Log(targetAngle);
         CurrentAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
         transform.rotation = Quaternion.Euler(0, CurrentAngle, 0);
    }

    

    // private void OnAim(InputAction.CallbackContext ctx) {
    //     if (AimCoroutine != null)
    //         StopCoroutine(AimCoroutine);
    //     isAiming = ctx.started;
    //     int targetWeight = isAiming ? 1 : 0;
    //     AimCoroutine = AnimateAim(targetWeight, 0.25f);
    //     StartCoroutine(AimCoroutine);
    // }
    //
    // private IEnumerator AnimateAim(float targetWeight, float duration) {
    //     float original = animator.GetLayerWeight(1);
    //     yield return CoroutineUtils.Lerp(duration, t => {
    //         animator.SetLayerWeight(1, Mathf.Lerp(original, targetWeight, t));
    //     });
    // }
}
