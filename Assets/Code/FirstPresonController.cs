using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPresonController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float movementSpeed = 2;
    [SerializeField] float jumpHeight = 10;
    [SerializeField] float groundCheckRadius = 0.4f;
    [SerializeField] float gravity = -10;
    [SerializeField] LayerMask groundLayerMask;
    float velocityY = 0;
    Vector3 move;
    bool isGrounded;

    CharacterController characterController;
    [SerializeField] InputActionReference movementAction;
    [SerializeField] InputActionReference jumpAction;
    [SerializeField] Transform groundCheck;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        jumpAction.action.started += (InputAction.CallbackContext obj) => {
            if(isGrounded)
            {
                velocityY = Mathf.Sqrt(jumpHeight * -2 * gravity);
                print("jump");
            }
        };
    }

    void Update()
    {
        #region Movement
        move = new Vector3(movementAction.action.ReadValue<Vector2>().x, 0,
            movementAction.action.ReadValue<Vector2>().y) * movementSpeed * Time.deltaTime;

        move = transform.TransformDirection(move); // move with player's rotation
        characterController.Move(move);
        #endregion

        #region Jump and gravity
        velocityY += gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, velocityY * Time.deltaTime));  // attract to ground

        if (isGrounded && velocityY < 0)
            velocityY = -2;
        #endregion
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayerMask);
    }
}
