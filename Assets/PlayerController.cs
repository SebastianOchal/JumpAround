using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject pivot;
    // Variables to control aim Character
    [SerializeField] private float rotationSpeed, angle;
    // Jump variables
    [SerializeField] private float jumpHeight;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void AimCharacter(float aimInputs)
    {
        float minRot = 20;
        float maxRot = 160;
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
    public void Jump(bool isGrounded)
    {
        Vector3 launchRot = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        Vector2 launchDirection = new Vector2(launchRot.x, Mathf.Abs(launchRot.y));
        if (isGrounded)
        {
            rb.AddForce(launchDirection * jumpHeight, ForceMode2D.Impulse);
        }
    }
}
