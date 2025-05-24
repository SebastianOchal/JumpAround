using UnityEngine;
using UnityEngine.UIElements;

public class PlayerChecks : MonoBehaviour
{
    // Checks Variables
    [SerializeField] private Transform check;
    [SerializeField] private Vector2[] groundRayPositions;
    [SerializeField] private int rayAmount;
    [SerializeField] private float groundRayLength, wallRayLength, rayOffset, rayStart;
    [SerializeField] private LayerMask groundLayer, wallLayer;
    [SerializeField] private float groundedCheckCooldown = 0.2f;
    [SerializeField] private float nextCheckTime = 0f;
    [SerializeField] public bool isGrounded, onLeftWall, onRightWall, canDash, canJump;

    void Update()
    {
        if (Time.time >= nextCheckTime)
            {
                DrawRays();
                isGrounded = GroundCheck();
                WallCheck();
            if (isGrounded)
                {
                canDash = true;
                }
            if (onLeftWall || onRightWall)
            {
                canDash = true;
                canJump = true;
            }
            else
            {
                canJump = false;
            }
                nextCheckTime = Time.time + groundedCheckCooldown;
            }
    }
    private void DrawRays()
    {
        int currentRays;
        groundRayPositions = new Vector2[rayAmount];
        for (currentRays = 0; currentRays < rayAmount; currentRays++)
        {
            groundRayPositions[currentRays] = new Vector2((check.position.x - rayStart) + (currentRays * rayOffset), check.position.y);
        }
    }
    bool GroundCheck()
    {
        foreach (Vector2 position in groundRayPositions)
        {
            Debug.DrawRay(position, Vector3.down * groundRayLength, Color.red);
            if (Physics2D.Raycast(position, Vector2.down, groundRayLength, groundLayer)) { 
            return true;
            }
        }
        return false;
    }
    void WallCheck()
    {
        onLeftWall = Physics2D.Raycast(check.transform.position, Vector2.left, wallRayLength, wallLayer);
        onRightWall = Physics2D.Raycast(check.transform.position, Vector2.right, wallRayLength, wallLayer);
        Debug.DrawRay(check.transform.position, Vector3.left * wallRayLength, Color.red);
        Debug.DrawRay(check.transform.position, Vector3.right * wallRayLength, Color.red);
    }
}
