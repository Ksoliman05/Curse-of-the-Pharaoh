using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FakeTeleport : MonoBehaviour
{
    public Transform teleportTarget; // The position where the player will be teleported.
    public GameObject player;        // Reference to the player GameObject.

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            // Teleport the player to the target position.
            CharacterController characterController = player.GetComponent<CharacterController>();

            if (characterController != null)
            {
                characterController.enabled = false;  // Disable before teleporting
                player.transform.position = teleportTarget.position;  // Teleport
                characterController.enabled = true;   // Re-enable after teleporting
            }
            else
            {
                // Fall back if CharacterController is not used
                player.transform.position = teleportTarget.position;
            }

        }
    }
}


