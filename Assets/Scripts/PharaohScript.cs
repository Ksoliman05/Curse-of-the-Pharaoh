using UnityEngine;
using TMPro;  // Include this for TextMeshPro support
using UnityEngine.UI;

public class Sphinx : MonoBehaviour
{
    public TextMeshPro floatingText; // Reference to the floating welcome text.
    public TextMeshPro riddleText;   // Reference to the riddle text.
    public TMP_InputField inputField; // Reference to the TextMeshPro Input Field where player types the answer.

    public string welcomeMessage = "You have reached the end of the desert!";
    public string riddle = "What is the secret phrase you have learned from the clues? Type it to escape the desert.";
    public string correctAnswer = "it201";  // The correct answer to the final riddle

    private bool isPlayerInRange = false; // Tracks if the player is within interaction range.
    private bool riddleAnswered = false;  // Tracks if the riddle has been answered correctly.

    private void Start()
    {
        // Ensure the riddle text and input field are inactive at the start
        riddleText.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false); // Initially, the input field is hidden

        // Show the welcome message
        floatingText.text = welcomeMessage;
        floatingText.gameObject.SetActive(true);
    }

    private void Update()
    {
        // If the player is in range and hasn't answered the riddle yet, show the riddle
        if (isPlayerInRange && !riddleAnswered && Input.GetKeyDown(KeyCode.E))
        {
            ShowRiddle();
        }

        // After showing the riddle, check if the player answers the riddle
        if (riddleAnswered && Input.GetKeyDown(KeyCode.Return)) // Player hits Return/Enter to submit the answer
        {
            string playerAnswer = inputField.text.Trim();  // Get the player's input from the InputField.
            CheckAnswer(playerAnswer);
        }
    }

    private void ShowRiddle()
    {
        // Hide the floating text and show the riddle text
        floatingText.gameObject.SetActive(false);
        riddleText.text = riddle;
        riddleText.gameObject.SetActive(true);

        // Activate the InputField for the player to type their answer
        inputField.gameObject.SetActive(true); // Now the input field becomes visible
        inputField.text = ""; // Clear any previous answer
        inputField.ActivateInputField();  // Focus on the input field immediately
        riddleAnswered = true;  // Player is now expected to answer the riddle
    }

    private void CheckAnswer(string answer)
    {
        if (answer.ToLower() == correctAnswer.ToLower()) // Ignore case when comparing the answer
        {
            // Congratulate the player for the correct answer and end the game
            riddleText.text = "Correct! You have escaped the desert!";
            inputField.gameObject.SetActive(false); // Hide the input field after correct answer
        }
        else
        {
            riddleText.text = "Incorrect answer! Try again.";
            inputField.text = ""; // Clear the InputField if the answer is incorrect.
            inputField.ActivateInputField();  // Keep focus on the InputField so they can try again.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Use the tag to check if the colliding object is the player
        {
            isPlayerInRange = true;
            floatingText.text = welcomeMessage; // Show the welcome message
            floatingText.gameObject.SetActive(true); // Activate the floating text when player enters
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Use the tag to check if the player is leaving
        {
            isPlayerInRange = false;
            floatingText.gameObject.SetActive(false); // Hide the welcome message if the player leaves
        }
    }
}
