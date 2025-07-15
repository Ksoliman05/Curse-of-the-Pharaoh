using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Include this for TextMeshPro support

public class NPCGuide : MonoBehaviour
{
    public TextMeshPro floatingText; // Reference to the floating welcome text.
    public TextMeshPro riddleText;   // Reference to the riddle text.
    public TextMeshPro pyramidTask;
    public GameObject player;        // Reference to the player GameObject.
    public GameObject door;          // Reference to the door GameObject.
    public TMP_InputField inputField; // Reference to the TextMeshPro Input Field where player types the answer.

    public string welcomeMessage = "Welcome, brave explorer! I am a former explorer but as you can see my time is over. To proceed, you must solve my riddle.";
    public string riddle = "I am the god of the afterlife and mummification. Who am I?";
    public string correctAnswer = "Anubis";  // The correct answer to the riddle

    private bool isPlayerInRange = false; // Tracks if the player is within interaction range.
    private bool riddleAnswered = false;  // Tracks if the riddle has been answered correctly.

    private void Start()
    {
        // Display the welcome message at the start.
        floatingText.text = welcomeMessage;

        // Ensure the riddle text is inactive at the start.
        riddleText.gameObject.SetActive(false);
        pyramidTask.gameObject.SetActive(false);


        // Ensure the door is initially closed.
        if (door != null)
        {
            door.SetActive(true);  // Door is visible and closed at the start.
        }

        // Ensure the InputField is inactive at the start.
        inputField.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && !riddleAnswered && Input.GetKeyDown(KeyCode.E))
        {
            ShowRiddle();
        }

        // After showing the riddle, check if the player answers the riddle.
        if (riddleAnswered && Input.GetKeyDown(KeyCode.Return)) // Player hits Return/Enter to submit the answer
        {
            string playerAnswer = inputField.text.Trim();  // Get the player's input from the InputField.
            CheckAnswer(playerAnswer);
        }
    }

    private void ShowRiddle()
    {
        // Hide the floating text and show the riddle text.
        floatingText.gameObject.SetActive(false);
        riddleText.text = riddle;
        riddleText.gameObject.SetActive(true);

        // Activate the InputField for the player to type their answer.
        inputField.gameObject.SetActive(true);
        inputField.text = ""; // Clear any previous answer.
        inputField.ActivateInputField();  // Focus on the input field immediately.
        riddleAnswered = true;  // Player is now expected to answer the riddle.
    }

    private void CheckAnswer(string answer)
    {
        if (answer.ToLower() == correctAnswer.ToLower()) // Ignore case when comparing the answer
        {
            OpenDoor(); // Open the door if the answer is correct
            pyramidTask.gameObject.SetActive(true);
            pyramidTask.text = "Correct. You will have to face Anubis inside. Enter the Pyramid from the right and Good Luck!";

        }
        else
        {
            riddleText.text = "Incorrect answer! Try again.";
            inputField.text = ""; // Clear the InputField if the answer is incorrect.
            inputField.ActivateInputField();  // Keep focus on the InputField so they can try again.
        }
    }

    private void OpenDoor()
    {
        if (door != null)
        {
            // Here you can either move the door or deactivate it to "open" it.
            // Example: Disable door's collider to make it passable
            Collider doorCollider = door.GetComponent<Collider>();
            if (doorCollider != null)
            {
                doorCollider.enabled = false;  // Disables the collider to allow passage.
            }

            // Optionally, animate the door moving or fading out.
            door.SetActive(false); // Hide the door or make it invisible after opening.
        }

        // Deactivate the input field and riddle text after the answer.
        inputField.gameObject.SetActive(false);
        riddleText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = true;

            // Show the welcome message when the player enters.
            floatingText.gameObject.SetActive(true);
            floatingText.text = welcomeMessage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = false;

            // Do nothing when the player leaves, so text remains visible.
        }
    }
}