using UnityEngine;
using UnityEngine.InputSystem;

public class Walljump : MonoBehaviour
{

    private bool isWallJumping;
    public float jumpForce;
    public float jumpAmmount;
    void Start()
    {
        isWallJumping = false;
    }

    void Update()
    {
        if (IsGrounded())
        {
            jumpAmmount = 1;
        }
        if (isWallJumping && !IsGrounded() && jumpAmmount > 0)
        {
            if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit hitLeft, 0.8f))
            {
                Debug.Log("Vägg till vänster: " + hitLeft.collider.name);
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce + Vector3.right * jumpForce, ForceMode.Impulse);
            }
            else if (Physics.Raycast(transform.position, Vector3.right, out RaycastHit hitRight, 0.8f))
            {
                Debug.Log("Vägg till höger: " + hitRight.collider.name);
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce + Vector3.left * jumpForce, ForceMode.Impulse);
            }
            else if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hitForward, 0.8f))
            {
                Debug.Log("Vägg framåt: " + hitForward.collider.name);
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce + Vector3.back * jumpForce, ForceMode.Impulse);
            }
            else if (Physics.Raycast(transform.position, Vector3.back, out RaycastHit hitBack, 0.8f))
            {
                Debug.Log("Vägg bakåt: " + hitBack.collider.name);
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce + Vector3.forward * jumpForce, ForceMode.Impulse);
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

    public void OnWallJump(InputAction.CallbackContext context)
    {
        isWallJumping = context.ReadValueAsButton();
        if (isWallJumping)
        {
            jumpAmmount--;
        }
    }
}
