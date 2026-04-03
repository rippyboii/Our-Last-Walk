using UnityEngine;
using UnityEngine.InputSystem;

public class Dog_movement : MonoBehaviour
{
    public InputActionAsset inputActions;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    private InputAction crouchAction;

    private Vector2 moveVector;
    private Vector2 lookVector;

    private Rigidbody rb;
    private Animator animator;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float rotationSpeed = 100f;

    private InputActionMap playerMap;
    private bool isActive = true;

    private void Awake()
    {
        playerMap = inputActions.FindActionMap("Player");

        moveAction = playerMap.FindAction("Move");
        jumpAction = playerMap.FindAction("Jump");
        lookAction = playerMap.FindAction("Look");
        crouchAction = playerMap.FindAction("Crouch");

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    
    private void OnEnable()
    {
        playerMap.Enable();
    }

    private void OnDisable()
    {
        playerMap.Disable();
    }

    private void Update()
    {
        if (!isActive) return;

        moveVector = moveAction.ReadValue<Vector2>();
        lookVector = lookAction.ReadValue<Vector2>();

        if (jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
        if (crouchAction.IsPressed()){
            Crouch();
        }

    }

    private void FixedUpdate()
    {
        if (!isActive) return;

        Move();
        Rotate();
    }

    private void Move()
    {
        Vector3 move = (transform.forward * moveVector.y + transform.right * moveVector.x) * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + move);

        animator.SetFloat("Speed", moveVector.magnitude);
    }

    private void Rotate()
    {
        float rotation = lookVector.x * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turn = Quaternion.Euler(0, rotation, 0);

        rb.MoveRotation(rb.rotation * turn);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetTrigger("Jump");
    }

    private void Crouch()
    {
        //idk how to do this / what we want it to do
        animator.SetTrigger("Crouch");
        animator.SetBool("IsCrouching", true);
    }

    public void Active(bool active)
    {
        isActive = active;
        if (active)
            playerMap.Enable();
        else
            playerMap.Disable();
    }
    
}