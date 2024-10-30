using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ComputerPaddle : MonoBehaviour {

    [SerializeField] private Rigidbody ball;
    [SerializeField] private float moveSpeed = 40.0f;

    private Vector3 STARTING_PADDLE_POSITION = new Vector3(3f, 1.0f, -0.1f);
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {


        if (ball.velocity.x > 0f) {
            // Ball coming towards computer paddle
            if (ball.position.y > transform.position.y) {
                // Ball position is higher than the computer paddle, move paddle up
                rb.AddForce(Vector3.up * moveSpeed);
            } else if (ball.position.y < transform.position.y) {
                // Ball position is lower than computer paddle, move paddle down
                rb.AddForce(Vector3.down * moveSpeed);
            }
        } else {
            // Ball is going away from computer paddle, center computer paddle
            if (transform.position.y > 1f) {
                rb.AddForce(Vector3.down * moveSpeed);
            } else if (transform.position.y < 1f) {
                rb.AddForce(Vector3.up * moveSpeed);
            }
        }

    }

    public void ResetPaddle() {
        rb.position = STARTING_PADDLE_POSITION;
        rb.velocity = Vector3.zero;
    }

}
