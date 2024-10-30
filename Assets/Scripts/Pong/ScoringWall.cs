using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringWall : MonoBehaviour {

    [SerializeField] private PongManager pongManager;

    private string PLAYER_WALL_TAG = "PlayerWall";
    private string COMPUTER_WALL_TAG = "ComputerWall";

    private void OnCollisionEnter(Collision collision) {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null) {
            if (gameObject.tag == PLAYER_WALL_TAG) {
                pongManager.ComputerScore();
            } else if (gameObject.tag == COMPUTER_WALL_TAG) {
                pongManager.PlayerScore();
            }
        }
    
    }

}
