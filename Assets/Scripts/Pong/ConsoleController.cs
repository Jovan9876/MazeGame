using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ConsoleController : MonoBehaviour {

    [SerializeField] private GameObject debugConsolePanel;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject ball;
    [SerializeField] private TMP_InputField commandInputField;
    [SerializeField] private TMP_Text logText;
    [SerializeField] private ScrollRect scrollRect;

    private bool showConsole;
    private InputActions inputActions;
    private InputAction consoleOn;
    private InputAction consoleOff;
    private InputAction executeCommand;
    private string logHistory = "";


    private void Awake() {
        debugConsolePanel.SetActive(false);
        inputActions = new InputActions();
        consoleOn = inputActions.Console.ToggleConsole;
        consoleOff = inputActions.Console.CloseConsole;
        executeCommand = inputActions.Console.ExecuteCommand;
    }

    private void OnEnable() {
        consoleOn.Enable();
        consoleOn.performed += ToggleConsoleOn;
        consoleOff.Enable();
        consoleOff.performed += ToggleConsoleOff;
        executeCommand.Enable();
        executeCommand.performed += ExecuteCommand;
    }

    private void OnDisable() {
        consoleOn.Disable();
        consoleOn.performed -= ToggleConsoleOn;
        consoleOff.Disable();
        consoleOff.performed -= ToggleConsoleOff;
    }

    private void Update() {
        ToggleConsole();
    }

    private void ToggleConsole() {
        debugConsolePanel.SetActive(showConsole);
        if (showConsole) {
            commandInputField.ActivateInputField();
        }
    }

    private void ToggleConsoleOn(InputAction.CallbackContext context) {
        showConsole = true;
    }

    private void ToggleConsoleOff(InputAction.CallbackContext context) {
        showConsole = false;
    }

    private void ExecuteCommand(InputAction.CallbackContext context) {
        ParseCommand();
    }

    private void ParseCommand() {

        string command = commandInputField.text;
        commandInputField.text = ""; // Clear input field

        if (command.StartsWith("/")) {
            if (command.StartsWith("/help")) {

                AddToLog($"/setBG 'color' to set the background color");
                AddToLog($"/setWin 'int' to set score needed to win");
                AddToLog($"/setBall 'color' to set ball color");

            } else if (command.StartsWith("/setBG ")) {

                string colorName = command.Split(' ')[1];
                background.GetComponent<Renderer>().material.color = ColorFromString(colorName);
                AddToLog($"Changing background color to {colorName}");

            } else if (command.StartsWith("/setWin ")) {

                string winAmount = command.Split(' ')[1];
                PongManager.winScore = int.Parse(winAmount);
                AddToLog($"New score to win: {winAmount}");

            } else if (command.StartsWith("/setBall ")) {

                string colorName = command.Split(' ')[1];
                ball.GetComponent<Renderer>().material.color = ColorFromString(colorName);
                AddToLog($"Changing ball color to {colorName}");
            }
        }
    }

    // Adds a message to the command log
    private void AddToLog(string message) {

        logHistory = message + "\n" + logHistory;  // Add new log at the top
        logText.text = logHistory;  // Update the text component

        // Ensure the scroll area stays at the bottom to show the latest command
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    private Color ColorFromString(string colorName) {

        Color color;

        // Attempt to parse user input for a color
        if (ColorUtility.TryParseHtmlString(colorName, out color)) {
            return color;
        }

        // Invalid color entered
        return Color.white;
    }

}
