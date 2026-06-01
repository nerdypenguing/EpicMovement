using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wallrun : MonoBehaviour
{

    [Header("Wallrun")]
    [SerializeField] private Transform orientation;
    private Rigidbody rb;

    [Header("Detection")]
    private float wallDistance = 1f;

    private bool wallLeft;
    private bool wallRight;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float wallrunFov;
    [SerializeField] private float wallrunFovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public float wallRunForce;
    public float minJumpHeight;
    private float wallRunTimer;
    public float maxWallRunTime;
    private BetterMovement bm;




    public float tilt { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bm = GetComponent<BetterMovement>();
    }

    void Update()
    {

        CheckWall();


    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance, wallLayer);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance, wallLayer);
    }

    private bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, groundLayer);
    }
    /*
    private bool StateMachine()
    {
        if ((wallLeft || wallRight) && bm.isWalking && CanWallRun())
        {
            if (!bm.isWallRunning)
            {
                StartWallRun();
            }
        }
        else if (bm.isWallRunning)
        {
            StopWallRun();
        }
    }*/

    private void StartWallRun()
    {
        rb.useGravity = false;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        bm.isWallRunning = true;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        bm .isWallRunning = false;
         rb.useGravity = true;
    }

    private void WallRunMovement()
    {

    }
}


/*
IsWallrunning();
CheckWall();


if (wallLeft)
{
    Debug.DrawLine(transform.position, transform.position - orientation.right * wallDistance, Color.green);
}
else
{
    Debug.DrawLine(transform.position, transform.position - orientation.right * wallDistance, Color.red);
}
if (wallRight)
{
    Debug.DrawLine(transform.position, transform.position + orientation.right * wallDistance, Color.green);
}
else
{
    Debug.DrawLine(transform.position, transform.position + orientation.right * wallDistance, Color.red);
}

void IsWallrunning()
{
    rb.useGravity = false;
    if (wallLeft)
        tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
    else if (wallRight)
        tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
    else
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);

    if (wallLeft || wallRight)
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallrunFov, wallrunFovTime * Time.deltaTime);
    }
    else
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallrunFovTime * Time.deltaTime);
    }
}
*/