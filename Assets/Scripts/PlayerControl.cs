using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour {
    [SerializeField] private float _speed;
    [SerializeField] private float _turnSmoothTime;
    
    
    private PlayerInputActions _inputs;
    private CharacterController _controller;

    private InputAction _movementAction;

    private float _turnSmoothVelocity;

    private void OnEnable() {
       _inputs = new PlayerInputActions();
       _inputs.Enable();
       _movementAction = _inputs.Player.Move;
    }

    private void OnDisable() {
        _inputs.Disable();
    }
    void Start() {
        _controller = GetComponent<CharacterController>();
    }

    private void Update() {
        Vector2 movementInput = _movementAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        _controller.Move(movement * (_speed * Time.deltaTime));

        if (movementInput.magnitude >= 0.1f) {
            Debug.Log(movement);
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            // Debug.Log(targetAngle);
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
