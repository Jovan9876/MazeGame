using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PongManager : MonoBehaviour {

    public static int winScore = 2;

    [SerializeField] private Ball ball;
    [SerializeField] private PlayerPaddle playerPaddle;
    [SerializeField] private ComputerPaddle computerPaddle;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI computerScoreText;
    [SerializeField] private TextMeshProUGUI winnerText;

    private int _playerScore = 0;
    private int _computerScore = 0;

    public void PlayerScore() {
        _playerScore++;
        playerScoreText.text = _playerScore.ToString();
        Winner();
        ResetGame();
    }

    public void ComputerScore() {
        _computerScore++;
        computerScoreText.text = _computerScore.ToString();
        Winner();
        ResetGame();
    }

    public void ResetGame() {
        playerPaddle.ResetPaddle();
        ball.ResetBall();
        computerPaddle.ResetPaddle();

    }

    private void Winner() {
        if (_computerScore >= winScore) {
            winnerText.text = "Computer Wins!";
            Invoke("ChangeScene", 1);
        } else if (_playerScore >= winScore) {
            winnerText.text = "Player Wins!";
            Invoke("ChangeScene", 1);
        }
    }

    private void ChangeScene() {
        GameSceneManager.Instance.ReturnToMaze();
    }

}
