using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movementInput;
    public float moveSpeed;
    public float baseSpeed;

    public bool isWalking;
    public bool isSprinting;
    public bool isCrouching;
    public bool isDashing;

    public float crouchMultiplyer;
    public float crouchSpeed;

    public float sprintSpeed;
    public float speedMultiplyer;
    public float dashForce;

    public float smoothTurn;

    private bool tryingToJump;
    public float ammountOfJumps;
    private float jumpsLeft;
    public float jumpForce;

    private bool velocityChange;

    private Rigidbody rb;
    public Transform cameraHead;
    public Transform cameraCrouch;
    public Transform playerCamera;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpsLeft = ammountOfJumps;
        moveSpeed = baseSpeed;
        sprintSpeed = baseSpeed * speedMultiplyer;
        crouchSpeed = baseSpeed * crouchMultiplyer;
    }

    private void FixedUpdate()
    {
        float fallSpeed = rb.linearVelocity.y;

        Vector3 moveForceLocal = new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed;
        Vector3 moveForceGlobal = transform.TransformVector(moveForceLocal);
        moveForceGlobal.y = fallSpeed;
        Vector3 smoothMove = Vector3.Lerp(rb.linearVelocity, moveForceGlobal, smoothTurn);
        rb.linearVelocity = smoothMove;


        if (isDashing)
        {
            rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);
        }
        isDashing = false;


        if (tryingToJump && jumpsLeft > 0)
            {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpsLeft--;
            }
        tryingToJump = false;


        if (!IsGrounded())
        {
            moveSpeed = baseSpeed * 0.5f;
            rb.AddForce(Vector3.down * 60f, ForceMode.Acceleration);
            if (isSprinting)
            {
                moveSpeed = sprintSpeed * 0.5f;
            }
        }
        if (IsGrounded())
        {
            jumpsLeft = ammountOfJumps;
        }

        if (isCrouching)
        {
            moveSpeed = crouchSpeed;
            playerCamera.transform.position = cameraCrouch.position;

        }
        else if (!isCrouching)
        {
            playerCamera.transform.position = cameraHead.position;
            if (isSprinting && IsGrounded())
            {
                moveSpeed = sprintSpeed;
            }
            else if (!isSprinting && !isWalking)
            {
                moveSpeed = baseSpeed;
            }
        }

    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector2.down, out hit, 0.8f))
        {
            Debug.Log("Står på " + hit.collider.name);
            Debug.DrawLine(transform.position, hit.point, Color.green, 0.5f);
            return true;
        }
        Debug.Log("Är i luften");
        Debug.DrawRay(transform.position, Vector2.down * 0.8f, Color.red, 0.5f);

        return false;
    }
    
    public bool VelocityChange()
        {
        if (movementInput.x != 0 || movementInput.y != 0)
        {
            return true;
        }
        return false;
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

    public void OnMove(InputAction.CallbackContext context)
    {
            movementInput = context.ReadValue<Vector2>();
            isWalking = movementInput != Vector2.zero;
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