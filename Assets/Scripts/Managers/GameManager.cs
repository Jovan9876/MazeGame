using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    private MazeGenerator mazeGenerator;
    [SerializeField] private GameObject playerPreab;
    [SerializeField] private GameObject enemyPrefab;
    private const string PLAYER_GAMEOBJECT_NAME = "Player";

    public int startX = 0;
    public int startY = 0;
    public int mazeWidth = 5;
    public int mazeHeight = 5;
    public float cellSize = 3f;
    public int enemyX;
    public int enemyY;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject deathTextPanel;
    [SerializeField] private TMP_Text finishText;
    [SerializeField] private TMP_Text deathText;
    public int score = 0;
    [SerializeField] private TMP_Text scoreText;

    private void Awake() {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SoundManager.Init();
    }

    private void Start() {

        // Hides the cursor...
        Cursor.visible = false;

        // Releases the cursor
        //Cursor.lockState = CursorLockMode.None;

        // Locks the cursor
        //Cursor.lockState = CursorLockMode.Locked;

        // Confines the cursor
        Cursor.lockState = CursorLockMode.Confined;

        mazeGenerator = GetComponent<MazeGenerator>();
        mazeGenerator.GenerateMaze(mazeWidth, mazeHeight, startX, startY, cellSize);
        SpawnPlayersAfterMazeGeneration();
        InputHandler.Instance.OnMenuToggled += ToggleMenu;

    }

    private void SpawnPlayersAfterMazeGeneration() {
        // Instantiate the player prefab at the start position
        Instantiate(playerPreab, new Vector3(startX * cellSize, 0, startY * cellSize), Quaternion.identity);

        // Calculate the opposite corner from the player's starting position
        enemyX = mazeHeight - 1 - startX; 
        enemyY = mazeWidth - 1 - startY;

        // Spawn the enemy at the opposite corner
        Instantiate(enemyPrefab, new Vector3(enemyX * cellSize, 0f, enemyY * cellSize), Quaternion.identity);
    }


    public void DisplayFinish() {
        finishText.gameObject.SetActive(true);
    }

    public void DisplayDeath() {
        deathTextPanel.gameObject.SetActive(true);
        deathText.gameObject.SetActive(true);
    }

    public void Reset() {
        finishText.gameObject.SetActive(false);
        deathTextPanel.gameObject.SetActive(false);
        deathText.gameObject.SetActive(false);
        ResetScore();
    }

    public void AddScore() {
        score++;
        Debug.Log(score);
        scoreText.text = $"Score: {score.ToString()}";
    }

    public void ResetScore() {
        score = 0;
        scoreText.text = $"Score: {score.ToString()}";
    }

    public void HideScore() {
        scoreText.gameObject.SetActive(false);
    }

    public void ShowScore() {
        scoreText.gameObject.SetActive(true);
    }

    public void SetScore(int score) {
        this.score = score;
        scoreText.text = $"Score: {score.ToString()}";
    }

    private void ToggleMenu(bool isMenuOpen) {
        if (isMenuOpen) {
            menuPanel.gameObject.SetActive(true);
            Time.timeScale = 0f;
        } else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            menuPanel.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }

}
