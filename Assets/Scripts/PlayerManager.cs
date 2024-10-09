using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instance { get; private set; }

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
    }

    private void Update() {
        inputHandler.HandleAllInputs();
    }

    private void FixedUpdate() {
        playerMovement.HandleAllMovement();
    }

    public void ResetPlayer() {
        transform.SetPositionAndRotation(new Vector3(GameManager.Instance.startX * GameManager.Instance.cellSize, 0, GameManager.Instance.startY * GameManager.Instance.cellSize), Quaternion.identity);
        playerMovement.cameraObject.localRotation = Quaternion.identity;
    }

}
