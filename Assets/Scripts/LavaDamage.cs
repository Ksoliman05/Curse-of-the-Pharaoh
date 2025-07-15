using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LavaDamage : MonoBehaviour
{
    public TextMeshProUGUI healthText; // UI text to display player health
    public PyramidPickUps pyramidScript; // Reference to the pyramid script for initial health
    private int playerHealth; // Player's current health

    private void Start()
    {
        // Get health from PyramidPickUps if it exists
        if (pyramidScript != null)
        {
            playerHealth = pyramidScript.playerHealth;
        }

        // Initialize health display
        UpdateHealthText();
    }

    private void OnTriggerStay(Collider other)
    {
        // Continuously damage the player while in contact with lava
        if (other.CompareTag("Player"))
        {
            playerHealth--; // Reduce health
            UpdateHealthText();

            if (playerHealth <= 0)
            {
                Destroy(other.gameObject); // Destroy the player object
                Debug.Log("Game Over! Player died in the lava.");
            }
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
