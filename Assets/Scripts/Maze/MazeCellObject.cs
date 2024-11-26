using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MazeCellObject : MonoBehaviour {

    public bool IsVisited { get; private set; } = false;
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

    [SerializeField] private Material doorMaterial;
    [SerializeField] private Material doorClosedMaterial;

    private bool isFinish = false;
    public bool isDoor = false;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    private bool isDoorActive = true;
    private float doorActiveTime = 60f;
    private GameObject door;

    private void Update() {
        if (!isDoorActive) {
            door.GetComponent<Renderer>().material = doorClosedMaterial;
            doorActiveTime -= Time.deltaTime; // Decrement the timer

            // Check if 60 seconds have passed
            if (doorActiveTime <= 0f) {
                door.GetComponent<Renderer>().material = doorMaterial;
                isDoorActive = true; // Reactivate the door
                doorActiveTime = 60f; // Reset the timer
            }
        }
    }

    public void DestroyWall(string direction) {
        switch (direction) {
            case "Left":
                if (leftWall != null) {
                    Destroy(leftWall);
                    leftWall.SetActive(false);
                }
                if (insideLeftWall != null) {
                    Destroy(insideLeftWall);
                    insideLeftWall.SetActive(false);
                }
                break;
            case "Right":
                if (rightWall != null) {
                    Destroy(rightWall);
                    rightWall.SetActive(false);
                }
                if (insideRightWall != null) {
                    Destroy(insideRightWall);
                    insideRightWall.SetActive(false);
                }
                break;
            case "Front":
                if (frontWall != null) {
                    Destroy(frontWall);
                    frontWall.SetActive(false);
                }
                if (insideFrontWall != null) {
                    Destroy(insideFrontWall);
                    insideFrontWall.SetActive(false);
                }
                break;
            case "Back":
                if (backWall != null) {
                    Destroy(backWall);
                    backWall.SetActive(false);
                }
                if (insideBackWall != null) {
                    Destroy(insideBackWall);
                    insideBackWall.SetActive(false);
                }
                break;
        }
    }
    public void SetFinish() {
        // Add a collider to detect the player reaching this cell
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        isFinish = true;
    }

    public void SetDoor() {
        if (frontWall != null && frontWall.activeSelf) {
            frontWall.GetComponent<Renderer>().material = doorMaterial;
            insideFrontWall.GetComponent<Renderer>().material = doorMaterial;
            door = insideFrontWall;

        } else if (backWall != null && backWall.activeSelf) {
            backWall.GetComponent<Renderer>().material = doorMaterial;
            insideBackWall.GetComponent<Renderer>().material = doorMaterial;
            door = insideBackWall;

        } else if (rightWall != null && rightWall.activeSelf) {
            rightWall.GetComponent<Renderer>().material = doorMaterial;
            insideRightWall.GetComponent<Renderer>().material = doorMaterial;
            door = insideRightWall;
        } else if (leftWall != null && leftWall.activeSelf) {
            leftWall.GetComponent<Renderer>().material = doorMaterial;
            insideLeftWall.GetComponent<Renderer>().material = doorMaterial;
            door = insideLeftWall;
        }

        BoxCollider parentCollider = gameObject.AddComponent<BoxCollider>();
        Vector3 localDoorPosition = transform.InverseTransformPoint(door.transform.position);
        parentCollider.center = new Vector3(localDoorPosition.x, localDoorPosition.y, localDoorPosition.z);
        parentCollider.size = localDoorPosition.x != 0 ? new Vector3(0.05f, 3f, 3f) : new Vector3(3f, 3f, 0.05f);
        parentCollider.isTrigger = true;
        isDoor = true;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (isFinish) {
                GameManager.Instance.DisplayFinish();
            } else if (isDoor && isDoorActive) {
                isDoorActive = false;
                doorActiveTime = 60f;
                GameSceneManager.Instance.SaveAllData();
                GameSceneManager.Instance.SetMazeCells();
                GameSceneManager.Instance.TransitionToPong();
            }
        }
    }
}
