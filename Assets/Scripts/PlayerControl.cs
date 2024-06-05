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
    [SerializeField] public BoolDataEventChannelSO ToggleLaser;
    
    [FormerlySerializedAs("_inventory")] [SerializeField] public InventorySystemSO inventory; 
    
    public PlayerInputActions Inputs { get; private set; }
    public CharacterController Controller { get; private set; }
    public Animator Animator { get; private set; }


    public float Velocity { get; private set; }
    public Vector3 LocalVelocity => Quaternion.Euler(0, -CurrentAngle, 0) * MoveVelocity;
    private float _turnSmoothVelocity;
    private Vector2 _inputSmoothVelocity;
    public Vector2 CurrentInput { get; private set; }
    public bool HasInputThisFrame => CurrentInput.magnitude >= 0.1f;
    public Vector3 MoveVelocity { get; private set; }
    
    public float CurrentAngle { get; private set; }

    // State
    public PlayerBaseState CurrentState;
    public PlayerNormalState NormalState;
    public PlayerAimState AimState;

    private void OnEnable() {
       Inputs = new PlayerInputActions();
       Inputs.Enable();
       inventory.Clear();
    }
    

    private void OnDisable() {
       Inputs.Disable();
    }
    private void Start() {
        Controller = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();

        NormalState = new PlayerNormalState(this);
        AimState = new PlayerAimState(this);
        SwitchState(NormalState);
    }

    private void Update() {
        CurrentState.UpdateState();
    }
    
    public void SwitchState(PlayerBaseState newState) {
        CurrentState?.ExitState();
        CurrentState = newState;
        newState.EnterState();
    }


    public void ReadMovementInput() {
        Vector2 movementInput = Inputs.Player.Move.ReadValue<Vector2>();
        Vector2 movement = movementInput.normalized;
        CurrentInput = Vector2.SmoothDamp(CurrentInput, movement, ref _inputSmoothVelocity, _inputSmoothSpeed);
    }

    public void CalculateMoveVelocity(float currentMaxSpeed, float acceleration) {
        if (CurrentInput.magnitude >= 0.1f && Velocity < currentMaxSpeed) {
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

    public void SimpleMove(float currentMaxSpeed, float acceleration) {
        ReadMovementInput();
        CalculateMoveVelocity(currentMaxSpeed, acceleration);
        Move();
    }


    public void LookDir(Vector2 direction) {
         float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
         // Debug.Log(targetAngle);
         CurrentAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
         transform.rotation = Quaternion.Euler(0, CurrentAngle, 0);
    }
}
