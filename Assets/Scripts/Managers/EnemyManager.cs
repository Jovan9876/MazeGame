using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager Instance { get; private set; }

    private int health = 3;
    public GameObject enemyPrefab;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetEnemy() {
        Transform enemyTransform = transform;
        enemyTransform.SetPositionAndRotation(new Vector3(GameManager.Instance.enemyX * GameManager.Instance.cellSize, 0f, GameManager.Instance.enemyY * GameManager.Instance.cellSize), Quaternion.identity);
    }

    public void HitByBall() {
        health--;
        CheckHealth();
    }

    private void CheckHealth() {
        if (health <= 0) {
            // Move enemy super far away
            Vector3 newEnemyPosition = new Vector3(1000f, 0, 1000f);
            transform.position = newEnemyPosition;
            SoundManager.PlaySound(SoundManager.Sound.EnemyDie);
            Invoke("RespawnAtRandomLocation", 5f);
            Debug.Log("Respawning after 5 seconds");
        }
    }

    private void RespawnAtRandomLocation() {
        SoundManager.PlaySound(SoundManager.Sound.EnemyRespawn);
        // Generate random coordinates within the maze
        int randomX = Random.Range(0, GameManager.Instance.mazeWidth);
        int randomY = Random.Range(0, GameManager.Instance.mazeHeight);
        transform.position = new Vector3(randomX * GameManager.Instance.cellSize, 0f, randomY * GameManager.Instance.cellSize);
        health = 3; // Reset health
    }
}
