using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerChecks playerChecks;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerChecks = GetComponent<PlayerChecks>();
    }
    public float aimInput, jumpInput;
    public bool holdingJump;
    void OnAim(InputValue value) {
        aimInput = value.Get<float>();
    }
    void OnJump(InputValue value) {
        jumpInput = value.Get<float>();
        if (jumpInput == 1)
        {
            Debug.Log("HOLD");
            playerController.MoveDirection();
            playerController.Jump(playerChecks.isGrounded, playerChecks.canJump);
            if (playerChecks.canDash && !playerChecks.isGrounded) {
                playerController.AirDash(playerChecks.isGrounded);
                playerChecks.canDash = false;
            }
            holdingJump = true;
        }
        else
        {
            Debug.Log("RELEASE");
            
            holdingJump = false;
        }
    }
    private void Update()
    {
        playerController.AimCharacter(aimInput);
        playerController.JumpCut(playerChecks.isGrounded, holdingJump);
        playerController.WallSlide(playerChecks.onLeftWall, playerChecks.onRightWall,playerChecks.isGrounded);
    }
}
