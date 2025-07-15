using UnityEngine;
using UnityEngine.AI;

public class PetFollow : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;  // Reference to the NavMeshAgent
    public Transform player;            // Reference to the player's transform

    public float followDistance = 2.0f; // Distance the pet follows behind the player

    private void Start()
    {
        // Get the NavMeshAgent component on the pet
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("No NavMeshAgent found on the pet.");
            return;
        }

        navMeshAgent.stoppingDistance = 2.0f; // Set a reasonable stopping distance
        navMeshAgent.updateRotation = true;    // Make sure the pet rotates to face the movement direction
    }

    private void Update()
    {
        // Ensure the player is assigned before trying to follow
        if (player != null && navMeshAgent != null)
        {
            // Calculate the position to follow behind the player at a fixed distance
            Vector3 followPosition = player.position - player.forward * followDistance;
            // Set the pet's destination to the calculated position
            navMeshAgent.SetDestination(followPosition);
        }
    }
}
