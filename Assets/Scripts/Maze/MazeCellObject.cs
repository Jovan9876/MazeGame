using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeCellObject : MonoBehaviour {

    public bool IsVisited {  get; private set; } = false;
    public void Visit() {
        IsVisited = true;
        unvistedBlock.SetActive(false);
    }

    // Parent GameObject holding all walls
    [SerializeField] private GameObject mazeCell;        

    // All walls inside and out
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject insideFrontWall;
    [SerializeField] private GameObject backWall;
    [SerializeField] private GameObject insideBackWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject insideRightWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject insideLeftWall;

    // Placeholder gameobject in each cell
    [SerializeField] private GameObject unvistedBlock;

    public void DestroyWall(string direction) {
        switch (direction) {
            case "Left":
                Destroy(leftWall);
                Destroy(insideLeftWall);
                break;
            case "Right":
                Destroy(rightWall);
                Destroy(insideRightWall);
                break;
            case "Front":
                Destroy(frontWall);
                Destroy(insideFrontWall);
                break;
            case "Back":
                Destroy(backWall);
                Destroy(insideBackWall);
                break;
        }
    }

    public void SetFinish() {
        // Add a collider to detect the player reaching this cell
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            // Call the function to handle the player finishing the maze
            GameManager.Instance.DisplayFinish();
        }
    }



}
