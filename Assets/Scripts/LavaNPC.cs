using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro support

public class LavaLevelNPC : MonoBehaviour
{
    public TextMeshPro floatingText; // Reference to the floating welcome text
    public TextMeshPro instructionsText; // Reference to the instructions text
    public TMP_InputField inputField; // Input field for player interaction
    public GameObject player; // Reference to the player
    public GameObject door; // Reference to the door to open

    public string welcomeMessage = "Welcome to the lava challenge! Press E to learn the rules.";
    public string instructions = "The floor is lava! Avoid touching it and find your way to the end. Type 'ready' or 'yes' to proceed.";

    private bool isPlayerInRange = false; // Tracks if the player is in interaction range
    private bool hasReceivedInstructions = false; // Tracks if the player has received instructions

    private void Start()
    {
        // Ensure the texts are initially hidden
        floatingText.gameObject.SetActive(true);
        floatingText.text = welcomeMessage;
        instructionsText.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);

        // Ensure the door is initially closed
        if (door != null)
        {
            door.SetActive(true); // Door is visible and closed at the start
        }
    }

    private void Update()
    {
        // If the player is in range and presses E, display the instructions
        if (isPlayerInRange && !hasReceivedInstructions && Input.GetKeyDown(KeyCode.E))
        {
            ShowInstructions();
        }

        // Check if the player submits their readiness in the input field
        if (hasReceivedInstructions && Input.GetKeyDown(KeyCode.Return))
        {
            string playerAnswer = inputField.text.Trim().ToLower(); // Get and normalize the player's input
            CheckReadiness(playerAnswer);
        }
    }

    private void ShowInstructions()
    {
        // Hide the welcome message and show the instructions
        floatingText.gameObject.SetActive(false);
        instructionsText.text = instructions;
        instructionsText.gameObject.SetActive(true);

        // Show the input field for the player to confirm readiness
        inputField.gameObject.SetActive(true);
        inputField.text = ""; // Clear any previous input
        inputField.ActivateInputField(); // Focus the input field
        hasReceivedInstructions = true; // Indicate instructions are displayed
    }

    private void CheckReadiness(string answer)
    {
        // Check if the answer is either "ready" or "yes" (case insensitive)
        if (answer == "ready" || answer == "yes")
        {
            OpenDoor(); // Open the door if the answer is correct
            instructionsText.text = "Good luck! The door is now open.";
            inputField.gameObject.SetActive(false); // Hide the input field
        }
        else
        {
            instructionsText.text = "Please type 'ready' or 'yes' to proceed.";
            inputField.text = ""; // Clear the incorrect input
            inputField.ActivateInputField(); // Refocus the input field
        }
    }

    private void OpenDoor()
    {
        if (door != null)
        {
            // Deactivate the door to "open" it
            door.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = true;

            // Show the welcome message when the player enters the range
            floatingText.gameObject.SetActive(true);
            floatingText.text = welcomeMessage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = false;

            // Hide the welcome message when the player leaves
            floatingText.gameObject.SetActive(false);
        }
    }
}
