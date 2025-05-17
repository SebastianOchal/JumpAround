using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    //GameObjects,Components
    #region
    [Header("GameObjects")]
    [SerializeField] private Camera mainCam;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject pivot;
    #endregion
    // variables for AimAtMouse & LaunchControl
    #region
    //Launch Variables
    [Header("Launch Settings")]
    [SerializeField] private float launchBase;
    [SerializeField] private float launchForce;
    [SerializeField] private float maxLaunchForce;
    private Vector2 launchForceVec, launchDirection;
    private Vector3 launchRot;
    [SerializeField]private float angle, angleFlipped, angleMin,angleMax;
    private bool aimed, isPositive;
    #endregion
    // Raycasts,Layers,Bools & Check
    #region
    //Raycasts
    [Header("Ground Check (Raycast)")]
    [SerializeField] private Transform check;
    [SerializeField] private float groundCheckRadius, groundRayLength, sideRayLength;
    [SerializeField] private LayerMask groundLayer, wallLayer;
    public bool isGrounded, onRightWall, onLeftWall, onWall;
    #endregion
    // variables for jumpAffectors & wallJumps
    #region
    [SerializeField] private float gravityLow;
    [SerializeField] private float gravityHigh;
    [SerializeField] private float gravityOnWall;
    [SerializeField] private float gravitySwitchThreshold;
    private float regularGravity = 3f;
    #endregion
    public void OnJump(InputValue value)
    {
        float v = value.Get<float>();
        if (v == 1)
        {
            Debug.Log("HOLD");
            aimed = true;
        }
        else
        {
            Debug.Log("RELEASE");
            //launchControl();
            aimed = false;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Checks();
        if (!aimed)
        {
            aimCharacter();
        }
        jumpAffectors();
        wallJumps();
    }
    void aimCharacter()
    {
        //Vector2 direction = mousePos - transform.position;
        //// only gives values 0-180 and 0 - -180
        //angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //// converts angle to full 360
        //float normalizedAngle = (angle + 360f) % 360f;
        //// provides the opposite of angle in full 360 
        //angleFlipped = angle - angleMax;
        //if ((angle < angleMin) || (angle > angleMax))
        //{
        //    pivot.transform.rotation = Quaternion.Euler(0f, 0f, angleFlipped);
        //    isPositive = false;
        //}
        //else
        //{
        //    pivot.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        //    isPositive = true;
        //}
    }
    void launchControl()
    {
        if (isGrounded || onWall)
        {
            if (isPositive)
            {
                launchRot = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
            }
            else
            {
                launchRot = Quaternion.AngleAxis(angleFlipped, Vector3.forward) * Vector3.right;
            }
            //launchForceVec = mousePosStart - mousePosEnd;
            launchForce = Mathf.Clamp(Math.Abs(launchForceVec.sqrMagnitude) * launchBase, 0, maxLaunchForce);
            launchDirection = new Vector2(launchRot.x, Mathf.Abs(launchRot.y));
            rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
        }
    }
    void Checks()
    {
        isGrounded = Physics2D.CircleCast(pivot.transform.position, groundCheckRadius, -transform.up, groundRayLength, groundLayer);
        onRightWall = Physics2D.Raycast(check.position, Vector2.right, sideRayLength, wallLayer);
        onLeftWall = Physics2D.Raycast(check.position, Vector2.left, sideRayLength, wallLayer);
        onWall = onLeftWall || onRightWall;
    }
    void jumpAffectors()
    {
        if (rb.linearVelocity.y < -gravitySwitchThreshold)
        {
            // Falling
            rb.gravityScale = gravityHigh;
        }
        else
        {
            // At apex
            rb.gravityScale = regularGravity; // Optional smoothing
        }
    }
    void wallJumps()
    {
        if (!isGrounded && onWall)
        {
            rb.gravityScale = gravityOnWall;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (check != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pivot.transform.position - transform.up * groundRayLength, groundCheckRadius);
            Gizmos.DrawLine(check.position, check.position + Vector3.right * sideRayLength);
            Gizmos.DrawLine(check.position, check.position + Vector3.left * sideRayLength);
        }
    }
}
