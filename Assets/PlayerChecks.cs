using UnityEngine;

public class PlayerChecks : MonoBehaviour
{
    // Checks Variables
    [SerializeField] private Transform check;
    [SerializeField] private Vector2[] rayPositions;
    [SerializeField] private int rayAmount;
    [SerializeField] private float groundRayLength, rayOffset, rayStart;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public bool isGrounded;
    [SerializeField] public bool shouldCheck;
    [SerializeField] private float groundedCheckCooldown = 0.2f;
    [SerializeField] private float nextCheckTime = 0f;

    void Update()
    {
            if (Time.time >= nextCheckTime)
            {
                DrawRays();
                isGrounded = GroundCheck();
                nextCheckTime = Time.time + groundedCheckCooldown;
            }
    }
    private void DrawRays()
    {
        int currentRays;
        rayPositions = new Vector2[rayAmount];
        for (currentRays = 0; currentRays < rayAmount; currentRays++)
        {
            rayPositions[currentRays] = new Vector2((check.position.x - rayStart) + (currentRays * rayOffset), check.position.y);
        }
    }
    bool GroundCheck()
    {
        foreach (Vector2 position in rayPositions)
        {
            Debug.DrawRay(position, Vector3.down * groundRayLength, Color.red);
            if (Physics2D.Raycast(position, Vector2.down, groundRayLength, groundLayer)) { 
            return true;
            }
        }
        return false;
    }
}
