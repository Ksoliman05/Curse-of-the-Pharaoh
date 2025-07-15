using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PyramidPickUps : MonoBehaviour
{
    public TextMeshProUGUI healthText; // UI text to display player health
    public int playerHealth = 6; // Player's starting health
    public int pickupCount = 0; // Tracks the number of pickups collected
    public int totalPickups = 10; // Total number of pickups to collect
    public GameObject anubis; // Reference to Anubis (the enemy)
    public GameObject exitDoor; // Reference to the exit door
    public GameObject stairs; // Reference to the stairs leading to the next level
    public GameObject NPC2;
    public TextMeshProUGUI countText; // UI text to display collected count

    public AudioSource pickupSound;  // Reference to the AudioSource component for the pickup sound

    private void Start()
    {
        // Hide count text at the start
        countText.gameObject.SetActive(false);
        NPC2.gameObject.SetActive(false);

        // Ensure stairs are initially inactive
        if (stairs != null)
        {
            stairs.SetActive(false);
        }

        // Initialize health display
        UpdateHealthText();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Reduce health if colliding with Anubis
        if (other.gameObject.CompareTag("enemy"))
        {
            playerHealth--; // Reduce health
            UpdateHealthText(); // Update the health text in the UI

            if (playerHealth <= 0) // Check if the player has run out of health
            {
                Destroy(gameObject); // Destroy the player object
                countText.text = "You lose."; // Show lose message
                countText.gameObject.SetActive(true); // Make the text visible
            }
        }

        // Check if the object has the Pickup tag
        if (other.CompareTag("Pickup"))
        {
            // Disable the pickup object
            other.gameObject.SetActive(false);

            // Play the pickup sound
            if (pickupSound != null)
            {
                pickupSound.Play(); // Play the sound when the pickup is collected
            }

            // Increment the pickup count
            pickupCount++;
            UpdateCountText();

            // Check if the player has collected all pickups
            if (pickupCount >= totalPickups)
            {
                // Destroy Anubis and deactivate the exit door
                if (anubis != null)
                {
                    Destroy(anubis);
                }

                if (exitDoor != null)
                {
                    exitDoor.SetActive(false);
                }
                // Activate the stairs to the next level
                if(NPC2 != null){
                    NPC2.gameObject.SetActive(true);
                }

                if (stairs != null)
                {
                    stairs.SetActive(true);
                }
                countText.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateCountText()
    {
        if (countText != null)
        {
            // Update the UI text to display the current pickup count
            countText.gameObject.SetActive(true);
            countText.text = "Pickups: " + pickupCount + " / " + totalPickups;
        }
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + playerHealth.ToString();
        }
    }
}
