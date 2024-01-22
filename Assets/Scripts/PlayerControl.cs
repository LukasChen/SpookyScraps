using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour {
    [SerializeField] private float _speed;
    
    
    private PlayerInputActions _inputs;
    private CharacterController _controller;

    private InputAction _movementAction;

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
        Vector3 movementInput = _movementAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        _controller.Move(movement * (_speed * Time.deltaTime));
    }
}
