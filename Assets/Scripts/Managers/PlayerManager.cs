using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instance { get; private set; }

    ThrowProjectile projectile;
    PlayerMovement playerMovement;
    InputHandler inputHandler;
    private float deathMessageTime = 3f; // Time when the death message was displayed
    private bool isDead = false; // Flag to check if the player is dead


    private void Awake() {

        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        playerMovement = GetComponent<PlayerMovement>();
        inputHandler = GetComponent<InputHandler>();
        projectile = GetComponent<ThrowProjectile>();
    }

    private void Update() {
        inputHandler.HandleAllInputs();

        if (isDead) {
            deathMessageTime -= Time.deltaTime; // Decrement the timer
            if (deathMessageTime <= 0f) {
                PlayerManager.Instance.ResetPlayer(); // Reset player
                EnemyManager.Instance.ResetEnemy(); // Reset enemy
                GameManager.Instance.Reset(); // Reset the game state
                isDead = false; // Reactivate the door
                deathMessageTime = 3f; // Reset the timer
            }
        }

    }

    private void FixedUpdate() {
        playerMovement.HandleAllMovement();
    }

    public void ResetPlayer() {
        transform.SetPositionAndRotation(new Vector3(GameManager.Instance.startX * GameManager.Instance.cellSize, 0, GameManager.Instance.startY * GameManager.Instance.cellSize), Quaternion.identity);
    }

    public void HandleBallThrow() {
        projectile.ThrowBall();
    }

    private void OnCollisionEnter(Collision collision) {
        // Check if the collided object is on the "Enemy" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall")) {
            SoundManager.PlaySound(SoundManager.Sound.PlayerWallCollide);
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            if (!isDead) {
                isDead = true; 
                GameManager.Instance.DisplayDeath();
                deathMessageTime = 3f; 
            }
        }
    }


}
