using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public static EnemyManager Instance { get; private set; }

    private Animator animator;
    private Rigidbody enemyRigidbody;

    private float horizontalInput;
    private float verticalInput;
    public float moveSpeed = 3f;
    public float changeDirectionTime = 2f;
    private float directionChangeTimer;
    private bool isIdling = true;
    private float idleTime= 5f;
    private Vector3 movementDirection;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        enemyRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start() {
        ChangeDirection(); // Set initial direction
        directionChangeTimer = changeDirectionTime; // Initialize the timer
    }

    private void FixedUpdate() {

        if (isIdling) {
            // Count down the idle time
            idleTime -= Time.fixedDeltaTime;
            animator.SetFloat("InputX", 0f);
            animator.SetFloat("InputY", 0f);
            // When the idle time is over, start moving
            if (idleTime <= 0f) {
                isIdling = false;
                ChangeDirection(); // Set initial direction after idling
            }
        } else {
            // Update the timer
            directionChangeTimer -= Time.fixedDeltaTime;


            // If the timer has reached zero, change direction
            if (directionChangeTimer <= 0f) {
                ChangeDirection();
                directionChangeTimer = changeDirectionTime; // Reset the timer
            }

            // Move the enemy based on the movement direction
            enemyRigidbody.MovePosition(transform.position + movementDirection * moveSpeed * Time.fixedDeltaTime);

            // Rotate the enemy to face the movement direction
            if (movementDirection != Vector3.zero) // Only rotate if there is movement
            {
                Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * moveSpeed);
            }

            animator.SetFloat("InputX", horizontalInput);
            animator.SetFloat("InputY", verticalInput);
        }

    }

    private void ChangeDirection() {
        // Randomize the direction
        movementDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        // Set the horizontal and vertical inputs based on movement direction
        horizontalInput = movementDirection.x;
        verticalInput = movementDirection.z;
    }

    public void ResetEnemy() {
        transform.SetPositionAndRotation(new Vector3(GameManager.Instance.enemyX * GameManager.Instance.cellSize, 0f, GameManager.Instance.enemyY * GameManager.Instance.cellSize), Quaternion.identity);
    }

    private void Idle() {
        Invoke("", idleTime);
    }

}
