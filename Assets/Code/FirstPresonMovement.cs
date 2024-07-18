using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPresonMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float walkSpeed = 2;
    [SerializeField] float runSpeed = 5;
    [SerializeField] float jumpHeight = 10;
    [SerializeField] float ceilingCheckRadius = 0.4f;
    [SerializeField] float gravity = -10;
    [SerializeField] LayerMask ceilingLayerMask;
    float velocityY = 0;
    float movementSpeed;
    [SerializeField] float footstepDelayFactor = 1;
    bool hasLanded;

    [Tooltip("Ground attracting when player's on the ground")]
    const float DEFAULT_GROUND_ATTRACTING = -2;

    [Header("References")]
    CharacterController characterController;
    [SerializeField] InputActionReference movementAction;
    [SerializeField] InputActionReference jumpAction;
    [SerializeField] InputActionReference runAction;
    [SerializeField] Transform ceilingCheck; // if hit a 'ceiling' during jump, should immediately fall to the ground
    AudioSource source;

    [Header("Movement SFX")]
    [SerializeField] AudioClip footstep1;
    [SerializeField] AudioClip footstep2;
    [SerializeField] AudioClip land;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        movementSpeed = walkSpeed;

        #region Jump handle
        jumpAction.action.started += (InputAction.CallbackContext obj) =>
        {
            if(characterController.isGrounded)
            {
                velocityY = Mathf.Sqrt(jumpHeight * DEFAULT_GROUND_ATTRACTING * gravity);         
            }
        };
        #endregion

        #region Run handle
        runAction.action.started += (InputAction.CallbackContext obj) =>
        {
            if(characterController.isGrounded)
                movementSpeed = runSpeed;
        };

        runAction.action.canceled += (InputAction.CallbackContext obj) =>
        {
            movementSpeed = walkSpeed;
        };
        #endregion

        StartCoroutine(PlayFoodstepSFX());
    }

    void Update()
    {
        #region Movement
        Vector3 move = new Vector3(movementAction.action.ReadValue<Vector2>().x, 0,
            movementAction.action.ReadValue<Vector2>().y) * movementSpeed * Time.deltaTime;

        move = transform.TransformDirection(move); // move with player's rotation
        characterController.Move(move);
        #endregion

        #region Jump and gravity
        velocityY += gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, velocityY * Time.deltaTime));  // attract to ground

        if (characterController.isGrounded && velocityY < 0)
            velocityY = -2;
        #endregion

        #region Land sound
        if (characterController.isGrounded && !hasLanded)
        {
            source.PlayOneShot(land);
            hasLanded = true;
        }
        if(!characterController.isGrounded)
        {
            hasLanded = false;
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (Physics.CheckSphere(ceilingCheck.position, ceilingCheckRadius, ceilingLayerMask))
            velocityY = DEFAULT_GROUND_ATTRACTING;
    }

    IEnumerator PlayFoodstepSFX()
    {
        // if player walks plays footstep sounds
        int stepAmount = 0;
        while (true)
        {
            if (movementAction.action.ReadValue<Vector2>().x > 0 || movementAction.action.ReadValue<Vector2>().y > 0 && characterController.isGrounded)
            {
                stepAmount++;

                if (stepAmount % 2 == 0)
                    source.PlayOneShot(footstep2);
                else
                    source.PlayOneShot(footstep1);
            }
            yield return new WaitForSeconds(1 / (movementSpeed * footstepDelayFactor));
        }
    }
}
