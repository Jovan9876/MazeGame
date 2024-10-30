using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyWalls : MonoBehaviour {

    [SerializeField] private float bounceSpeed = 12f;
    [SerializeField] private GameObject ballObj;
    private string BALL_TAG = "Ball";


    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag(BALL_TAG)) {
            // Ball collided with wall
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 normal = collision.GetContact(0).normal;
            rb.AddForce(-normal * bounceSpeed);
        }
    }

}
