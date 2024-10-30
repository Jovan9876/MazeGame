using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float moveSpeed = 40.0f;

    private InputActions inputActions;
    private InputAction player1;
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputActions();
        player1 = inputActions.Pong.Movement;
    }

    private void OnEnable() {
        player1.Enable();
    }

    private void FixedUpdate() {
        Vector3 player1V3 = player1.ReadValue<Vector3>();
        Vector3 player1Move = new Vector3(0, player1V3.y * moveSpeed, 0);
        rb.AddForce(player1Move);
    }
    private void OnDisable() {
        player1.Disable();
    }

}
