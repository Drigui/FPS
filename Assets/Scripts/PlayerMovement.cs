using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //components
    private CharacterController characterController;
    private Transform cameraTransform;

    //movement and jump config parameters
    [SerializeField] private float speed = 5f;
    [SerializeField] private float multiplier = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = Physics.gravity.y;

    //Input fields for move and look actions
    private Vector2 moveInput;
    private Vector2 lookInput;

    //velocity and rotation var
    private Vector2 velocity;
    private float verticalVelocity;
    private float verticalRotation;

    //is sprinting state
    private bool isSprinting;

    //camera look sense and max angle to limit v rotation
    private float lookSensitivity = 1f;
    private float maxLookAngle = 1f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO manage player movement
        MovePlayer();
        //TODO manage camera rotation
    }

    /// <summary>
    /// receives move IS
    /// </summary>
    /// <param name="context"></param>
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    /// <summary>
    /// receive look input from inpout system
    /// </summary>
    /// <param name="context"></param>
    public void Look(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// receive jump IS and triggers jump if grounded
    /// </summary>
    /// <param name="context"></param>
    public void Jump(InputAction.CallbackContext context)
    {
        //if player is touching ground
        if (characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    /// <summary>
    /// receive Sprint from IS and change isSprinting state
    /// </summary>
    /// <param name="context"></param>
    public void Sprint(InputAction.CallbackContext context)
    {
        isSprinting = context.started || context.performed;
    }
    /// <summary>
    /// move jump player
    /// </summary>
    private void MovePlayer()
    {
        //falling down
        if (characterController.isGrounded)
        {
            //restart v velocity when touchs ground
            verticalVelocity = 0f;
        }
        else
        {
            //
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 move = new Vector3(0, verticalVelocity, 0);
        characterController.Move(move * Time.deltaTime);
        
        //movement
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = transform.TransformDirection(moveDirection);
        float targetSpeed = isSprinting ? speed * multiplier : speed;
        characterController.Move(moveDirection * targetSpeed * Time.deltaTime);

        //apply gravity constantly
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// camera rotation
    /// </summary>
    private void LookAround()
    {
        //horizontal rotation 
        float horizontalRotation = lookInput.x * lookSensitivity;
        transform.Rotate(Vector3.up * horizontalRotation);

        //vertical  rotation xaxis
        verticalRotation = lookInput.y * lookSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    

}
