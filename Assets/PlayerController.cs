using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject pivot;
    // Variables to control aim Character
    [SerializeField] private float rotationSpeed, angle, minRot, maxRot;
    // Jump variables
    [SerializeField] private float jumpHeight;
    private bool isMovingLeft, isMovingRight, isFalling;
    // Dash Variables
    [SerializeField] private float dashForce;
    // Wallslide variables
    [SerializeField] private float wallSlideSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void AimCharacter(float aimInputs)
    {
        Vector3 currentRotVector = pivot.transform.eulerAngles;
        Vector3 rotationAmount = Vector3.forward * aimInputs * rotationSpeed * Time.deltaTime;
        angle = currentRotVector.z;
        
        if ((angle > minRot) && (angle < maxRot))
        {
            pivot.transform.Rotate(rotationAmount);
        }
        else if (angle < minRot)
        {
            pivot.transform.rotation = Quaternion.Euler(0, 0, maxRot - 0.01f);
        }
        else if (angle > maxRot)
        {
            pivot.transform.rotation = Quaternion.Euler(0, 0, minRot + 0.001f);
        }
    }
    public void Jump(bool isGrounded, bool canJump)
    {
        Vector3 launchRot = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        Vector2 launchDirection = new Vector2(launchRot.x, Mathf.Abs(launchRot.y));
        if (isGrounded || canJump)
        {
            rb.AddForce(launchDirection * jumpHeight, ForceMode2D.Impulse);
        }
    }
    public void JumpCut(bool isGrounded, bool holdingJump) 
    {
        if (isGrounded) {
            rb.gravityScale = 3;
        }else if (!isGrounded && !holdingJump)
        {
            rb.gravityScale = 5;
        }else if (isFalling)
        {
            rb.gravityScale = 5;
        }
    }
    public void AirDash(bool isGrounded) {
        float upBoost = 3;
        if (isMovingLeft) {
            rb.AddForce(-transform.right * dashForce, ForceMode2D.Impulse);
            rb.AddForce(transform.up * upBoost, ForceMode2D.Impulse);
        } else if (isMovingRight) 
        {
            rb.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
            rb.AddForce(transform.up * upBoost, ForceMode2D.Impulse);
        }
    }
    public void WallSlide(bool onLeftWall, bool onRightWall, bool isGrounded)
    {
        if ((onLeftWall || onRightWall) && !isGrounded)
        {
            rb.gravityScale = 0.4f;
        }
        if (onLeftWall)
        {
            minRot = -20;
            maxRot = 60;
        }
        else if (onRightWall)
        {
            minRot = 120;
            maxRot = 200;
        }
        else {
            minRot = 20;
            maxRot = 160;
        }
    }
    public void MoveDirection()
    {
        float threshold = 0.01f;

        if (rb.linearVelocityY < -threshold)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }

        if (rb.linearVelocityX < -threshold)
        {
            isMovingLeft = true;
            isMovingRight = false;
        }
        else if (rb.linearVelocityX > threshold)
        {
            isMovingLeft = false;
            isMovingRight = true;
        }
        else
        {
            isMovingLeft = false;
            isMovingRight = false;
        }
    }
}
