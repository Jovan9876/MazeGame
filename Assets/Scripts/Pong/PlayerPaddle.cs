using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddle : MonoBehaviour {

    private Vector3 STARTING_PADDLE_POSITION = new Vector3(-3f, 1.0f, -0.1f);
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void ResetPaddle() {
        rb.position = STARTING_PADDLE_POSITION;
        rb.velocity = Vector3.zero;
    }

}
