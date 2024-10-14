using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectile : MonoBehaviour {

    [SerializeField] GameObject ballPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private Transform cameraObject;

    private Rigidbody ballRB;
    private float throwForce = 5f;


    public void ThrowBall() {
        Debug.Log("Performed");
        // Instantiate the ball at the throwPoint position and rotation
        GameObject ball = Instantiate(ballPrefab, throwPoint.position, throwPoint.rotation);

        ballRB = ball.GetComponent<Rigidbody>();

        if (ballRB != null) {
            Vector3 throwDirection = cameraObject.forward;
            ballRB.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }
    }

}
