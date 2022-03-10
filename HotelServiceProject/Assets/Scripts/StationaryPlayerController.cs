using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class StationaryPlayerController : MonoBehaviour
{
    // input feilds
    // Reference to the gerated input Action C# script
    private PlayerInput playerInputActions;
    private InputAction look;
    private InputAction movement;

    // movement feilds
    private Rigidbody rb;
    private new Collider collider;
    private Transform playerTrans;

    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxSpeed = 5f;

    private Vector3 forceDirection = Vector3.zero;
    private float distToGround;

    // camera feilds
    private CinemachineVirtualCamera virtualCam;
    [SerializeField] private Camera Cam;
    [SerializeField] private float rotSpeed;
    private float rotX, rotY;
    float camX, camY;

    // Start is called before the first frame update
    void Start()
    {
        // cache references
        rb = this.GetComponent<Rigidbody>();
        collider = this.GetComponent<CapsuleCollider>();
        playerTrans = this.GetComponent<Transform>();

        virtualCam = GetComponentInChildren<CinemachineVirtualCamera>();

        // get the distance to ground
        distToGround = collider.bounds.extents.y;
    }

    private void Awake()
    {
        // Input action asset is not static or gloabal
        // So we must make a new instance of the input action asset
        playerInputActions = new PlayerInput();
    }

    private void OnEnable()
    {
        // cache reference of the move variable
        movement = playerInputActions.Player.Move;
        look = playerInputActions.Player.Look;
        // We NEED to enable the movement
        movement.Enable();
        look.Enable();

        playerInputActions.Player.Jump.performed += DoJump;
        playerInputActions.Player.Jump.Enable();

        playerInputActions.Player.Fire.performed += DoFire;
        playerInputActions.Player.Fire.Enable();
    }

    // Reason for OnDisable(): events wont get called and
    // thus throw errors if the object is disabled
    private void OnDisable()
    {
        movement.Disable();
        look.Disable();
        playerInputActions.Player.Jump.Disable();
        playerInputActions.Player.Fire.Disable();
    }

    private void FixedUpdate()
    {
        // PLAYER MOVEMENT

        forceDirection.x += movement.ReadValue<Vector2>().x * movementForce;
        forceDirection.z += movement.ReadValue<Vector2>().y * movementForce;

        // Apply force to the rigid body
        rb.AddRelativeForce(forceDirection, ForceMode.Impulse);
        // once they let go of WASD the play will stop
        forceDirection = Vector3.zero;

        if (!isGrounded())
        {
            rb.velocity += Vector3.down * -Physics.gravity.y * 0.025f; ;
        }

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }
    }

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, (float)(distToGround + 0.1));
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        //Debug.Log("Jump!!!");
        if (isGrounded())
        {
            //Debug.Log("Jump Force!!!");
            forceDirection += Vector3.up * jumpForce;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update Camera X, Y for doFire()
        camX = Screen.width / 2;
        camY = Screen.height / 2;

        //now for the mouse rotation
        rotX += look.ReadValue<Vector2>().x * rotSpeed;
        rotY += look.ReadValue<Vector2>().y * rotSpeed;

        rotY = Mathf.Clamp(rotY, -90f, 90f);

        // look.ReadValue<Vector2>();
        virtualCam.transform.localRotation = Quaternion.Euler(-rotY, 0, 0f);
        playerTrans.localRotation = Quaternion.Euler(0, rotX, 0f);
    }

    private void DoFire(InputAction.CallbackContext obj)
    {
        Debug.Log("Bam");
        Ray ray = Cam.ScreenPointToRay(new Vector3(camX, camY, 0));
        //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow, 10.0f);

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {

        }
    }
}
