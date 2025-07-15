using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC2 : MonoBehaviour
{
    public TextMeshPro congratulationsText; // TextMeshPro for congratulations message.
    public TextMeshPro hintText;            // TextMeshPro for the hint message.
    public TextMeshPro teleportText;        // TextMeshPro for the teleport message.
    public GameObject player;               // Reference to the player GameObject.

    private bool isPlayerInRange = false;   // Tracks if the player is within interaction range.
    private int messageIndex = 0;           // Tracks the current message being displayed.

    private void Start()
    {
        // Initialize all messages to be inactive at the start.
        congratulationsText.gameObject.SetActive(false);
        hintText.gameObject.SetActive(false);
        teleportText.gameObject.SetActive(false);
    }

    private void Update()
    {
        // If player presses E while in range, show the next message in sequence.
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShowNextMessage();
        }
    }

    private void ShowNextMessage()
    {
        // Hide all messages first.
        congratulationsText.gameObject.SetActive(false);
        hintText.gameObject.SetActive(false);
        teleportText.gameObject.SetActive(false);

        // Display the current message based on the messageIndex.
        if (messageIndex == 0)
        {
            congratulationsText.gameObject.SetActive(true);
            congratulationsText.text = "Congratulations, explorer! You have made it to the afterlife! Press E for your next hint.";
        }
        else if (messageIndex == 1)
        {
            hintText.gameObject.SetActive(true);
            hintText.text = "Your first hint corresponds to the 10 pickups you just picked up: T10. Make sure not to forget it.";
        }
        else if (messageIndex == 2)
        {
            teleportText.gameObject.SetActive(true);
            teleportText.text = "The next challenge is far far away. Go up the stairs to find a magic teleportation cube. Be sure not to choose the wrong cube!";
        }

        // Increment the message index for the next interaction.
        messageIndex++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = true;

            // Display the first message immediately when the player enters.
            congratulationsText.gameObject.SetActive(true);
            congratulationsText.text = "Congratulations, explorer! You have made it to the afterlife! Press E for your next hint.";
            messageIndex = 1; // Set index to the second message for the next interaction.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = false;            

        }
    }
}
