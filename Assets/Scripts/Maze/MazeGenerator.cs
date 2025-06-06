using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
public class MazeGenerator : MonoBehaviour {

    public static MazeGenerator Instance { get; private set; }

    [SerializeField] private GameObject rootMazeObject;
    [SerializeField] private MazeCellObject mazeCellPrefab;

    private int mazeWidth;
    private int mazeHeight;
    private float cellSize; // Scale of quads
    private int startX;
    private int startY;

    public MazeCellObject[,] maze;

    private Stack<Vector2Int> cellStack = new Stack<Vector2Int>(); // Stack for backtracking

    private void Awake() {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public enum Direction {
        Forward,
        Left,
        Right,
        Back,
    }

    private bool IsCellValid(int x, int y) {
        // x, y cant be - stops left side out of bounds and top side out of bounds
        // mazeWidth - 1 stops right side out of bounds
        // mazeHeight - 1 stops bottom side out of bounds
        // .visited Make sure current cell hasnt been visited
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1 || maze[x,y].IsVisited ) return false;
        return true;
    }


    public void GenerateMaze(int mazeWidth, int mazeHeight, int startX, int startY, float cellSize) {

        this.mazeWidth = mazeWidth;
        this.mazeHeight = mazeHeight;
        this.cellSize = cellSize;
        this.startX = startX;
        this.startY = startY;

        maze = new MazeCellObject[mazeWidth, mazeHeight];

        for (int x = 0; x < mazeWidth; x++) {
            for (int y = 0; y < mazeHeight; y++) {
                maze[x, y] = Instantiate(mazeCellPrefab, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity, rootMazeObject.transform);
            }
        }

        GenerateMazePath();
        SetMaterials();
        SetPongDoor();
    }

    private void SetMaterials() {
        maze[mazeWidth - 1, mazeHeight - 1].SetFinish();
    }

    private void SetPongDoor() {
        // Pick a random cell in the maze
        int randomX = Random.Range(0, mazeWidth - 1);
        int randomY = Random.Range(0, mazeHeight - 1);

        maze[randomX, randomY].SetDoor();
    }

    public void GenerateMazePath() {
        // Start path generation from startX startY
        Vector2Int currentCell = new Vector2Int(startX, startY);
        maze[currentCell.x, currentCell.y].Visit();
        cellStack.Push(currentCell);

        while (cellStack.Count > 0) {
            // Get current cell from the stack
            currentCell = cellStack.Peek();

            // Get unvisited neighbors
            List<(Vector2Int, Direction)> unvisitedNeighbors = GetUnvisitedCells(currentCell);

            if (unvisitedNeighbors.Count > 0) {
                // Choose a random cell and direction
                var chosenNeighbor = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];

                // Hide the wall between currentCell and nextCell
                RemoveWallBetween(currentCell, chosenNeighbor.Item1, chosenNeighbor.Item2);

                // Move to the cell and visit
                maze[chosenNeighbor.Item1.x, chosenNeighbor.Item1.y].Visit();
                cellStack.Push(chosenNeighbor.Item1);
            } else {
                // Backtrack if no unvisited cells
                cellStack.Pop();
            }
        }


    }

    // Get unvisited neighbors and their directions
    private List<(Vector2Int, Direction)> GetUnvisitedCells(Vector2Int currentCell) {
        List<(Vector2Int, Direction)> unvisitedCells = new List<(Vector2Int, Direction)>();

        // Check in all directions and add valid neighbors
        if (IsCellValid(currentCell.x, currentCell.y + 1)) unvisitedCells.Add((new Vector2Int(currentCell.x, currentCell.y + 1), Direction.Forward)); // Forward
        if (IsCellValid(currentCell.x, currentCell.y - 1)) unvisitedCells.Add((new Vector2Int(currentCell.x, currentCell.y - 1), Direction.Back));    // Back
        if (IsCellValid(currentCell.x + 1, currentCell.y)) unvisitedCells.Add((new Vector2Int(currentCell.x + 1, currentCell.y), Direction.Right));   // Right
        if (IsCellValid(currentCell.x - 1, currentCell.y)) unvisitedCells.Add((new Vector2Int(currentCell.x - 1, currentCell.y), Direction.Left));    // Left

        return unvisitedCells;
    }

    private void RemoveWallBetween(Vector2Int current, Vector2Int neighbor, Direction direction) {
        // Remove walls based on direction using the string equivalent
        switch (direction) {
            case Direction.Forward:
                maze[current.x, current.y].DestroyWall("Front");
                maze[neighbor.x, neighbor.y].DestroyWall("Back");
                break;
            case Direction.Back:
                maze[current.x, current.y].DestroyWall("Back");
                maze[neighbor.x, neighbor.y].DestroyWall("Front");
                break;
            case Direction.Right:
                maze[current.x, current.y].DestroyWall("Right");
                maze[neighbor.x, neighbor.y].DestroyWall("Left");
                break;
            case Direction.Left:
                maze[current.x, current.y].DestroyWall("Left");
                maze[neighbor.x, neighbor.y].DestroyWall("Right");
                break;
        }
    }


}