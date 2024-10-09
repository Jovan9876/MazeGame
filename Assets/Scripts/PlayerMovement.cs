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
    [SerializeField] private Transform cameraObject;

    private Vector3 moveDirection;
    private float movementSpeed = 5f;
    private float rotationSpeed = 350f;


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

        // Rotate player left/right from mouse input -- camera automatically follows
        //transform.Rotate(Vector3.up * mouseX);
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0f, mouseX, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Up and down of camera handled from inputhandler attached to cinemachine camera
    }

    private void HandlePhase() {
        playerCollider.enabled = inputHandler.phase;
    }

}
