using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour {

    private InputHandler inputHandler;
    private Rigidbody playerRigidbody;
    private Animator animator;
    private Collider playerCollider;
    public Transform cameraObject;

    private Vector3 moveDirection;
    private float movementSpeed = 5f;
    private float rotationSpeed = 250f;
    private float xRotation = 0f;


    private void Awake() {
        inputHandler = GetComponent<InputHandler>();
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();
    }

    public void HandleAllMovement() {
        HandlePhase();
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement() {

        moveDirection = cameraObject.forward * inputHandler.verticalInput;
        moveDirection += cameraObject.right * inputHandler.horizontalInput;

        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection *= movementSpeed;

        playerRigidbody.velocity = moveDirection;

        animator.SetFloat("InputX", inputHandler.horizontalInput);
        animator.SetFloat("InputY", inputHandler.verticalInput);

    }

    private void HandleRotation() {

        float mouseX = inputHandler.horizontalMouseInput * rotationSpeed * Time.deltaTime;
        float mouseY = inputHandler.verticalMouseInput * rotationSpeed * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraObject.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

    }

    private void HandlePhase() {
        playerCollider.enabled = inputHandler.phase;
    }

}
