using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerCam : MonoBehaviour
{

    [SerializeField] private float sensitivity;

    private Vector2 mouseInput;
    private float xPitch;
    private float yPitch;

    public Transform head;

    public Camera cam;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        yPitch -= mouseInput.y * sensitivity * Time.deltaTime;
        xPitch += mouseInput.x * sensitivity * Time.deltaTime;
        yPitch = Mathf.Clamp(yPitch, -90f, 90f);

        transform.localEulerAngles = new Vector3(0f, xPitch, 0f);
        //transform.rotation = Quaternion.Euler(0, xPitch, 0f);
        head.transform.localEulerAngles = new Vector3(yPitch, 0f, 0f);

        if (GetComponent<BetterMovement>().isSprinting)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 75, Time.deltaTime * 5);
        }
        else if (!GetComponent<BetterMovement>().isSprinting && !GetComponent<BetterMovement>().isWalking)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60, Time.deltaTime * 5);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }

}