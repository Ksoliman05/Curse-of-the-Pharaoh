using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Include this for TextMeshPro support
using UnityEngine.AI;  // Include for NavMeshAgent


public class NPC3 : MonoBehaviour
{
    public TextMeshPro floatingText; // Reference to the floating welcome text.
    public TextMeshPro riddleText;   // Reference to the riddle text.
    public TextMeshPro pyramidTask;
    public GameObject player;        // Reference to the player GameObject.
    public GameObject door;          // Reference to the door GameObject.
    public TMP_InputField inputField; // Reference to the TextMeshPro Input Field where player types the answer.
    
    public GameObject petPrefab;     // Reference to the pet prefab.
    private GameObject pet;          // Reference to the instantiated pet.
    
    public float fixedZDistance = 5f;  // The fixed distance for pet instantiation on the Z-axis (relative to the player).
    
    public string welcomeMessage = "Hey you made it to the maze. Press E for your next hint";
    public string riddle = "The next hint corresponds to the number of cubes you had to choose from: 2. To Enter the Maze enter all your clues in the order you received them";
    public string correctAnswer = "T102";  // The correct answer to the riddle

    private bool isPlayerInRange = false; // Tracks if the player is within interaction range.
    private bool riddleAnswered = false;  // Tracks if the riddle has been answered correctly.
    private bool petInstantiated = false;  // Tracks if the pet has been instantiated already.

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

        // Make the pet follow the player if it's instantiated
        if (pet != null)
        {
            FollowPlayer();
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
            pyramidTask.text = "Correct. Enter the maze and be careful of black cubes! Find the right cube that will take you to the Pharaoh!";

            // Instantiate the pet at a fixed Z distance in front of the player only if not already instantiated
            if (!petInstantiated)
            {
                InstantiatePetWithFixedZDistance();
                petInstantiated = true;  // Set the flag to prevent further pet instantiations
            }
            floatingText.gameObject.SetActive(false);  // Explicitly ensure floating text stays off

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

            door.SetActive(false); // Hide the door or make it invisible after opening.
        }

        // Deactivate the input field and riddle text after the answer.
        inputField.gameObject.SetActive(false);
        riddleText.gameObject.SetActive(false);
    }

    // Instantiate pet at a fixed Z distance in front of the player
    private void InstantiatePetWithFixedZDistance()
    {
        // Instantiate the pet prefab at a fixed Z distance in front of the player
        if (petPrefab != null && player != null)
        {
            Vector3 spawnPosition = player.transform.position + player.transform.forward * fixedZDistance; // Fixed Z distance
            pet = Instantiate(petPrefab, spawnPosition, Quaternion.identity);  // Instantiate at calculated position

            // Ensure the pet is on a valid NavMesh
            NavMeshAgent navMeshAgent = pet.GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                // Find the nearest point on the NavMesh
                NavMeshHit hit;
                if (NavMesh.SamplePosition(spawnPosition, out hit, 5f, NavMesh.AllAreas)) // Search within a 5 unit radius
                {
                    // Move the pet to the valid position on the NavMesh
                    pet.transform.position = hit.position;

                    // Set initial destination for the pet
                    navMeshAgent.destination = player.transform.position;
                }
                else
                {
                    Debug.LogWarning("Failed to place pet on the NavMesh. Ensure the position is on a walkable surface.");
                }
            }
        }
    }

    // Make the pet follow the player
    private void FollowPlayer()
    {
        // Ensure the pet has a NavMeshAgent component
        NavMeshAgent navMeshAgent = pet.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            // Continuously update the destination to the player's position
            navMeshAgent.destination = player.transform.position;
        }
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
