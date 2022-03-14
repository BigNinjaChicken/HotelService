using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;
using UnityEngine.UI;

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

    // fire feilds
    [SerializeField] private float interactDistance = 10f;
    private bool talkingCustomer = false;
    [SerializeField] private GameObject interactObject;
    [SerializeField] private TextMeshProUGUI chatGUI;
    private BasicCustomerController customerScript = null;
    private int chatIndex = 0;

    // aim train feilds
    private bool startedAimTraining = false;
    [SerializeField] private GameObject paperObject;
    [SerializeField] private GameObject stampMarker;
    private bool hitMarker = false;
    public LayerMask IgnoreMe;

    [SerializeField] private GameObject allQueueNodes;
    private List<QueueNode> queueNodes;

    // flash light feilds
    public float totalPower = 0;
    [SerializeField] private int BatteryCharge = 30;
    [SerializeField] private float shootDist = 10f;
    [SerializeField] private int shootCost = 4;
    [SerializeField] private TextMeshProUGUI powerGUI;
    [SerializeField] private GameObject lightBeam;
    [SerializeField] private GameObject flashLightWhole;

    // Room Serivce Feilds
    [SerializeField] private GameObject allRoomNodes;
    private List<RoomNode> roomNode;
    [SerializeField] private GameObject allServiceNumbers;
    private List<TextMeshPro> serviceText;
    public int servicePoints;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // cache references
        rb = this.GetComponent<Rigidbody>();
        collider = this.GetComponent<CapsuleCollider>();
        playerTrans = this.GetComponent<Transform>();

        virtualCam = GetComponentInChildren<CinemachineVirtualCamera>();

        queueNodes = new List<QueueNode>(allQueueNodes.GetComponentsInChildren<QueueNode>());
        roomNode = new List<RoomNode>(allRoomNodes.GetComponentsInChildren<RoomNode>());
        serviceText = new List<TextMeshPro>(allServiceNumbers.GetComponentsInChildren<TextMeshPro>());

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

        playerInputActions.Player.Action.performed += DoAction;
        playerInputActions.Player.Action.Enable();

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
        playerInputActions.Player.Action.Disable();
        playerInputActions.Player.Fire.Disable();
    }

    private void FixedUpdate()
    {
        if (!talkingCustomer)
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

        // Flash Light
        totalPower -= Time.fixedDeltaTime;
        if (totalPower <= 0)
        {
            totalPower = 0;
            lightBeam.SetActive(false);
        } 
        else
        {
            lightBeam.SetActive(true);
        }
    }

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, (float)(distToGround + 0.1));
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (!talkingCustomer)
        {
            //Debug.Log("Jump!!!");
            if (isGrounded())
            {
                //Debug.Log("Jump Force!!!");
                forceDirection += Vector3.up * jumpForce;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!talkingCustomer)
        {
            // Update Camera X, Y for doAction()
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

        powerGUI.text = "Watts: " + (int)totalPower;
    }

    private void DoFire(InputAction.CallbackContext obj)
    {
        if (talkingCustomer)
        {
            Ray ray1 = Cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit1;
            if (Physics.Raycast(ray1, out hit1, 100, ~IgnoreMe))
            {
                if (hit1.collider.gameObject.CompareTag("MarkerHitBox"))
                {
                    hitMarker = true;
                }
            }
        } 
        else
        {
            Ray ray2 = Cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit2;
            if (Physics.Raycast(ray2, out hit2, 100))
            {
                if (totalPower > shootCost)
                {
                    totalPower -= shootCost;

                    if (hit2.collider.gameObject.CompareTag("Ghost"))
                    {
                        GameObject hitGhost = hit2.collider.gameObject;
                        if (Vector3.Distance(gameObject.transform.position, hitGhost.transform.position) < shootDist)
                        {
                            hitGhost.GetComponent<GhostController>().die();
                        }
                    }
                }
            }
        }

        
    }

    private void DoAction(InputAction.CallbackContext obj)
    {
        Ray ray = Cam.ScreenPointToRay(new Vector3(camX, camY, 0));
        //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow, 10.0f);

        if (talkingCustomer)
        {
            chatIndex++;
            if (customerScript.customerChat.Length > chatIndex)
            {
                chatGUI.text = customerScript.customerChat[chatIndex];
            }
            else
            {
                if (!startedAimTraining)
                {
                    doAimTrainingGame(customerScript.aimCount);
                }
            }
        }

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("BasicCustomer"))
            {
                GameObject hitCustomer = hit.collider.gameObject;

                if (Vector3.Distance(playerTrans.position, hitCustomer.transform.position) < interactDistance)
                {
                    if (hitCustomer.GetComponent<BasicCustomerController>().queuePos == 0 && !talkingCustomer)
                    {
                        chatIndex = 0;
                        customerScript = hitCustomer.GetComponent<BasicCustomerController>();

                        interactObject.SetActive(true);
                        flashLightWhole.SetActive(false);

                        talkingCustomer = true;

                        chatGUI.text = customerScript.customerChat[chatIndex];
                    }
                }
            }

            if (hit.collider.gameObject.CompareTag("RoomNode"))
            {
                GameObject hitDoor = hit.collider.gameObject;
                if (Vector3.Distance(playerTrans.position, hitDoor.transform.position) < interactDistance)
                {
                    for (int i = 0; i < roomNode.Count; i++)
                    {
                        if (roomNode[i].transform.position == hitDoor.transform.position)
                        {
                            for (int j = 0; j < serviceText.Count; j++)
                            {
                                if (serviceText[j].text.Equals("" + (i + 1)))
                                {
                                    Debug.Log("POINT");
                                    servicePoints++;

                                    serviceText[j].text = "-";
                                }
                            }
                        }
                    }
                }
            }
        }


        
    }

    private void doAimTrainingGame(int aimTrainingCount)
    {
        startedAimTraining = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        chatGUI.text = ""; // turn off the chat

        paperObject.SetActive(true);

        StartCoroutine(waitTillMouse(aimTrainingCount));
        
    }

    IEnumerator waitTillMouse(int aimTrainingCount)
    {
        for (int i = 0; i < aimTrainingCount; i++)
        {
            stampMarker.transform.localPosition = new Vector3(Random.Range(-5.22f, 3.84f), 0.02f, Random.Range(-5.1f, 3.8f));
            yield return new WaitUntil(() => hitMarker);
            hitMarker = false;
        }

        givePower(BatteryCharge);
        resetInteraction();
    }

    void givePower(int power)
    {
        totalPower += power;
    }

    void resetInteraction()
    {
        startedAimTraining = false;
        chatIndex = 0;
        paperObject.SetActive(false);
        interactObject.SetActive(false);
        flashLightWhole.SetActive(true);
        queueNodes[customerScript.queuePos].nodeTaken = false;
        Destroy(customerScript.gameObject);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        customerScript = null;
        talkingCustomer = false;
    }

    
}
