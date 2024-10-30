using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private const float START_TIME = 1.0f;
    private const float STARTING_FORCE = 50.0f;

    private Vector3 STARTING_BALL_POSITION = new Vector3(0.1f, 1.0f, -0.1f);
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Start() {
        ResetBall();
    }

    void AddStartingForce() {
        float x = Random.value < 0.5 ? -1.0f : 1.0f; // Randomly choose if ball goes left or right
        float y = Random.value < 0.5 ? Random.Range(-1.0f, -0.5f) : Random.Range(0.5f, 1.0f); // Randomly choose if ball goes up or down
        rb.AddForce(new Vector3(x * STARTING_FORCE, y * STARTING_FORCE, 0.0f));
    }

    public void ResetBall() {
        rb.position = STARTING_BALL_POSITION;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Invoke("AddStartingForce", START_TIME);
    }

}
