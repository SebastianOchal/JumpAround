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
        playerController.Jump(playerChecks.isGrounded);
        if (jumpInput == 1)
        {
            Debug.Log("HOLD");
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
    }
}
