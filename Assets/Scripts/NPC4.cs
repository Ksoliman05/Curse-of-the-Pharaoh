using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // For TextMeshPro support.

public class NPC4 : MonoBehaviour
{
    public GameObject player;                // Reference to the player.
    public GameObject sphinxPrefab;          // Reference to the Sphinx prefab to spawn.
    public Transform raycastOrigin;          // The point from which the ray will be cast (usually above the ground).
    public float raycastDistance = 10f;      // Maximum raycast distance to find the spawn point.
    public TMP_InputField inputField;        // Input field for the player's answer.
    public TextMeshPro riddleText;           // The NPC's riddle display text.

    public string riddle = "I am in Time, and I am in Tie. I am in FIsh, I am also sometimes only one. What Am I?";
    public string correctAnswer = "i";       // Correct answer to the riddle.

    private bool isPlayerInRange = false;    // Tracks if the player is within range to interact with the NPC.
    private bool riddleAnswered = false;     // Prevents multiple Sphinx spawns.

    private void Start()
    {
        // Show the riddle at the start.
        riddleText.text = riddle;

        // Ensure the InputField is inactive initially.
        inputField.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Allow the player to interact with the NPC to start answering the riddle.
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !riddleAnswered)
        {
            // Activate the InputField for the player to type their answer.
            inputField.gameObject.SetActive(true);
            inputField.text = ""; // Clear any previous input.
            inputField.ActivateInputField(); // Focus on the InputField.
        }

        // Check the player's answer when they press Enter, but only if the InputField is active.
        if (Input.GetKeyDown(KeyCode.Return) && inputField.gameObject.activeSelf && !riddleAnswered)
        {
            string playerAnswer = inputField.text.Trim();
            CheckAnswer(playerAnswer);
        }
    }

    private void CheckAnswer(string answer)
    {
        if (answer.ToLower() == correctAnswer.ToLower()) // Case-insensitive comparison.
        {
            riddleText.text = "Correct! Prepare to face the Sphinx!";
            InstantiateSphinx(); // Spawn the Sphinx.
            gameObject.SetActive(false); // Disable this NPC after answering.
            riddleAnswered = true; // Prevent further interactions.
            inputField.DeactivateInputField(); // Stop the input field from taking input.
            inputField.gameObject.SetActive(false); // Hide the InputField.
        }
        else
        {
            riddleText.text = "Incorrect. Try again!";
            inputField.text = ""; // Clear the InputField.
            inputField.ActivateInputField(); // Refocus on the InputField.
        }
    }

    private void InstantiateSphinx()
    {
        // Debugging: Ensure the correct prefab is assigned and check the reference.
        if (sphinxPrefab == null)
        {
            Debug.LogError("Sphinx Prefab is not assigned!");
            return; // Exit if no Sphinx prefab is assigned.
        }

        if (raycastOrigin != null)
        {
            Ray ray = new Ray(raycastOrigin.position, Vector3.down); // Cast ray downward.
            RaycastHit hit;

            Debug.DrawRay(raycastOrigin.position, Vector3.down * raycastDistance, Color.red, 2f); // Debug the ray.

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                // Spawn the Sphinx at the ground position.
                Instantiate(sphinxPrefab, hit.point, Quaternion.identity);
                Debug.Log("Sphinx spawned at: " + hit.point);
            }
            else
            {
                Debug.LogWarning("No suitable ground found for Sphinx spawn!");
            }
        }
        else
        {
            Debug.LogWarning("Raycast origin is not set!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !riddleAnswered)
        {
            isPlayerInRange = true;
            riddleText.gameObject.SetActive(true); // Show the riddle text when the player is in range.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = false;
            inputField.gameObject.SetActive(false); // Hide the InputField when the player leaves.
        }
    }
}
