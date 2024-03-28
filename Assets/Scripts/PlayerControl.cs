using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour {
    [SerializeField] private float _speed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _acceleration = 0.5f;
    [SerializeField] private float _runAcceleration = 1;
    [SerializeField] private float _turnSmoothTime;
    
    [SerializeField] private InventorySystemSO _inventory; 
    
    private PlayerInputActions _inputs;
    private CharacterController _controller;
    private Animator _animator;

    private InputAction _movementAction;

    private float _velocity;
    private float _turnSmoothVelocity;

    private void OnEnable() {
       _inputs = new PlayerInputActions();
       _inputs.Enable();
       _movementAction = _inputs.Player.Move;
       _inputs.Player.Interact.performed += OnInteract;
       _inventory.Clear();
    }

    private void OnDisable() {
        _inputs.Disable();
    }
    void Start() {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        Vector2 movementInput = _movementAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        bool isRunning = _inputs.Player.Sprint.ReadValue<float>() == 1;

        float acceleration = isRunning ? _runAcceleration : _acceleration;
        float maxSpeed = isRunning ? _runSpeed : _speed;
        
        if (movementInput.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            // Debug.Log(targetAngle);
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            if (_velocity > _speed && !isRunning) {
                _velocity = Mathf.Clamp(_velocity - acceleration * Time.deltaTime,0, _runSpeed);
            }
            else {
                _velocity = Mathf.Clamp(_velocity + acceleration * 2 * Time.deltaTime, 0, maxSpeed);
            }
        }
        else {
            _velocity = Mathf.Clamp(_velocity - acceleration * 2 * Time.deltaTime,0, _runSpeed);
        }


        float animatorVel = _velocity / _runSpeed;
        _animator.SetFloat("velocity", animatorVel);

        Vector3 move = new Vector3(movement.x, Physics.gravity.y, movement.z);
        _controller.Move(move * (_velocity * Time.deltaTime));
    }

    private void OnInteract(InputAction.CallbackContext ctx) {
        if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out var hit, 3)) {
            if (hit.transform.TryGetComponent<InventoryItem>(out InventoryItem item)) {
                item.HandlePickup(_inventory);
            }
        }
    }
}
