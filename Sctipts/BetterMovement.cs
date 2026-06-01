using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BetterMovement : MonoBehaviour
{
    private Rigidbody rb;
    public Transform cameraHead;
    public Transform cameraCrouch;
    public Transform playerCamera;

    private Vector2 movementInput;

    [Header("Movement")]
    [SerializeField] private float baseSpeed;

    [Header("Speeds")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float isInAir;
    [SerializeField] private float wallRunSpeed;

    [SerializeField] public bool tryingToJump;
    [SerializeField] public bool isWalking;
    [SerializeField] public bool isSprinting;
    [SerializeField] public bool isCrouching;
    [SerializeField] public bool isWallRunning;

    [Header("Multipliers")]
    [SerializeField] private float crouchMultiplyer;
    [SerializeField] private float speedMultiplyer;
    [SerializeField] private float airMultiplyer;
    [SerializeField] private float jumpForce;

    [Header("Gravity")]
    [SerializeField] private float minGravity;
    [SerializeField] private float maxGravity;
    [SerializeField] private float gravityScale;

    [Header("Dash")]
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashForce;

    [Header("Drag")]
    public float groundDrag = 8f;
    public float airDrag = 5f;
    public float dragMultiplier = 0.02f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        moveSpeed = baseSpeed;
        sprintSpeed = baseSpeed * speedMultiplyer;
        crouchSpeed = baseSpeed * crouchMultiplyer;
        isInAir = baseSpeed * airMultiplyer;


    }

    // clam magnitude
    void Update()
    {
        Walking();
        Crouching();
        ApplyDrag();


        if (!IsGrounded())
        {
            moveSpeed = baseSpeed * 0.5f;
            //rb.AddForce(Vector3.down * gravityScale, ForceMode.Acceleration);
            rb.AddForce(movementInput.normalized * moveSpeed * isInAir * airMultiplyer, ForceMode.Acceleration);
            if (isSprinting)
            {
                moveSpeed = sprintSpeed * 0.5f;
            }
            tryingToJump = false;
        }
        else if (isSprinting)
        {
            moveSpeed = sprintSpeed;
        }
        else if (isCrouching)
        {
            moveSpeed = crouchSpeed;
        }
        else if (isWallRunning)
        {
            moveSpeed = wallRunSpeed;
        }
        else
        {
            moveSpeed = baseSpeed;
        }

        if (tryingToJump && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            tryingToJump = false;
        }

        if (isDashing)
        {
            rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);
        }
        isDashing = false;
    }


    void ApplyDrag()
    {
        Vector3 drag = -rb.linearVelocity.normalized * Mathf.Pow( rb.linearVelocity.magnitude,2) * dragMultiplier;
        rb.AddForce(drag, ForceMode.Acceleration);

        if (IsGrounded())
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
        }
    }


    bool Walking()
    {
        rb.AddForce(transform.forward * movementInput.y * moveSpeed, ForceMode.Acceleration);
        rb.AddForce(transform.right * movementInput.x * moveSpeed, ForceMode.Acceleration);
        return isWalking;

    }

    void Crouching()
    {   
        if (isCrouching)
            {
                playerCamera.position = Vector3.Lerp(playerCamera.position, cameraCrouch.position, Time.deltaTime * 8);
            }
            else
            {
                playerCamera.position = Vector3.Lerp(playerCamera.position, cameraHead.position, Time.deltaTime * 8);
            }
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector2.down, out hit, 0.8f))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green, 0.5f);
            return true;
        }
        Debug.DrawRay(transform.position, Vector2.down * 0.8f, Color.red, 0.5f);
        return false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        isWalking = movementInput != Vector2.zero;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            tryingToJump = true;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        isCrouching = context.ReadValueAsButton();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isDashing = true;
        }
    }
}
