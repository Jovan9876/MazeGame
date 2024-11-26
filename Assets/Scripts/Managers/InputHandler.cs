using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour {
    public static InputHandler Instance { get; private set; }
    InputActions inputActions;
    private Vector2 movementInput;
    private Vector2 mouseInput;

    public float horizontalInput { get; private set; }
    public float verticalInput { get; private set; }
    public float horizontalMouseInput { get; private set; }
    public float verticalMouseInput { get; private set; }
    public bool ResetButtonPressed { get; private set; }
    public bool phase { get; private set; } = true;

    public event Action<bool> OnMenuToggled;
    private bool menu = false;

    private void OnEnable() {
        if (inputActions == null) {
            inputActions = new InputActions();
            inputActions.Player.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            inputActions.Player.Movement.canceled += i => movementInput = Vector2.zero;
            inputActions.Player.MouseMovement.performed += i => mouseInput = i.ReadValue<Vector2>();
            inputActions.Player.MouseMovement.canceled += i => mouseInput = Vector2.zero;
            inputActions.Player.Phase.performed += TogglePhase;
            inputActions.Player.Reset.performed += Reset_performed;
            inputActions.Player.Projectile.performed += ThrowBallPerformed;
            inputActions.Player.Menu.performed += state => ToggleMenu();
        }
        inputActions.Enable();
    }
    private void OnDisable() {
        inputActions.Disable();
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }


    private void ThrowBallPerformed(InputAction.CallbackContext context) {
        PlayerManager.Instance.HandleBallThrow();
    }

    public void Reset_performed(InputAction.CallbackContext obj) {
        PlayerManager.Instance.ResetPlayer();
        EnemyManager.Instance.ResetEnemy();
        GameManager.Instance.Reset();
    }


    public void HandleAllInputs() {
        HandleMovementInput();
        HandleMouseInput();
    }

    private void HandleMovementInput() {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;
    }

    private void HandleMouseInput() {
        horizontalMouseInput = mouseInput.x;
        verticalMouseInput = mouseInput.y;
    }

    private void TogglePhase(InputAction.CallbackContext context) {
        phase = !phase;
    }

    private void ToggleMenu() {
        menu = !menu;
        OnMenuToggled?.Invoke(menu);
    }
}
