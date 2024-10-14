using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instance { get; private set; }
    [SerializeField] private GameObject playerPreab;

    ThrowProjectile projectile;
    PlayerMovement playerMovement;
    InputHandler inputHandler;

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
        }
    }


}
