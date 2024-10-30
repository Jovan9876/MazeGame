using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

[Serializable]
public class MazeState {
    public float[] playerPosition;  // Stores x, y, z as [x, y, z]
    public float[] playerRotation;  // Stores x, y, z, w as [x, y, z, w]
    public float[] enemyPosition;
    public float[] enemyRotation;
}

public class GameSceneManager : MonoBehaviour {
    private MazeState mazeState = new MazeState();
    [SerializeField] private GameObject rootMazeObject;
    public static GameSceneManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(rootMazeObject);
    }

    public MazeState LoadData(string relativePath) {
        string path = Application.persistentDataPath + relativePath;

        if (!File.Exists(path)) {
            Debug.LogError($"Cannot load file at {path}. File does not exist!");
            throw new FileNotFoundException($"{path} does not exist!");
        }

        try {
            MazeState data;
            data = JsonConvert.DeserializeObject<MazeState>(File.ReadAllText(path));
            return data;
        } catch (Exception e) {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    public bool SaveData(string relativePath) {

        string path = Application.persistentDataPath + relativePath;
        Debug.Log(path);
        try {
            if (File.Exists(path)) {
                Debug.Log("Data exists. Deleting old file and writing a new one!");
                File.Delete(path);
            } else {
                Debug.Log("Writing file for the first time!");
            }
            using FileStream stream = File.Create(path);

            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(mazeState));
            return true;
        } catch (Exception e) {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public void SetPlayerCoords() {
        mazeState.playerPosition = new float[3] {
            PlayerManager.Instance.transform.position.x,
            PlayerManager.Instance.transform.position.y,
            PlayerManager.Instance.transform.position.z
        };
        mazeState.playerRotation = new float[4] {
            PlayerManager.Instance.transform.rotation.x,
            PlayerManager.Instance.transform.rotation.y,
            PlayerManager.Instance.transform.rotation.z,
            PlayerManager.Instance.transform.rotation.w
        };
    }

    public void SetEnemyCoords() {
        mazeState.enemyPosition = new float[3] {
            EnemyManager.Instance.transform.position.x,
            EnemyManager.Instance.transform.position.y,
            EnemyManager.Instance.transform.position.z
        };
        mazeState.enemyRotation = new float[4] {
            EnemyManager.Instance.transform.rotation.x,
            EnemyManager.Instance.transform.rotation.y,
            EnemyManager.Instance.transform.rotation.z,
            EnemyManager.Instance.transform.rotation.w
        };
    }

    public void SetMazeCells() {
        rootMazeObject.SetActive(false);
    }

    public void TransitionToPong() {
        SaveData("/data.json");
        PlayerManager.Instance.gameObject.SetActive(false);
        EnemyManager.Instance.gameObject.SetActive(false);
        SceneManager.LoadScene("Pong");
    }

    public void ReturnToMaze() {
        mazeState = LoadData("/data.json");
        // Convert float arrays back to Vector3 and Quaternion
        Vector3 playerPos = new Vector3(mazeState.playerPosition[0], mazeState.playerPosition[1], mazeState.playerPosition[2]);
        Quaternion playerRot = new Quaternion(mazeState.playerRotation[0], mazeState.playerRotation[1], mazeState.playerRotation[2], mazeState.playerRotation[3]);

        Vector3 enemyPos = new Vector3(mazeState.enemyPosition[0], mazeState.enemyPosition[1], mazeState.enemyPosition[2]);
        Quaternion enemyRot = new Quaternion(mazeState.enemyRotation[0], mazeState.enemyRotation[1], mazeState.enemyRotation[2], mazeState.enemyRotation[3]);

        PlayerManager.Instance.transform.SetPositionAndRotation(playerPos, playerRot);
        EnemyManager.Instance.transform.SetPositionAndRotation(enemyPos, enemyRot);

        SceneManager.LoadScene("Maze");
        PlayerManager.Instance.gameObject.SetActive(true);
        EnemyManager.Instance.gameObject.SetActive(true);
        rootMazeObject.SetActive(true);
    }

}