using System;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    Rigidbody playerRigidbody;
    
    public LayerMask groundLayerMask;
    
    private Vector3 moveDirection;
    private Transform cameraObject;

    public bool isSprinting;
    public bool isGrounded;

    public int jumpRemaining;
    public int maxJumps = 2;

    public float jumpForce = 7f;

    public float walkingSpeed = 3f;
    public float movementSpeed = 7f;
    public float sprintingSpeed = 10f;
    public float rotationSpeed = 15f;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        CheckIsGrounded();
        HandleMovement();
        HandleRotation();
        HandleJump();
    }

    private void HandleMovement()
    {
        Vector3 movementVel;
        
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.y = 0;
        moveDirection.Normalize();

        if (isSprinting)
        {
            movementVel = moveDirection * sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount < 0.5f)
            {
                movementVel = moveDirection * walkingSpeed;
            }
            else
            {
                movementVel = moveDirection * movementSpeed;
            }
        }
        
        movementVel.y = playerRigidbody.linearVelocity.y;
        playerRigidbody.linearVelocity = movementVel;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection += cameraObject.right * inputManager.horizontalInput;
        targetDirection.y = 0;
        targetDirection.Normalize();
        
        if(targetDirection == Vector3.zero)
            targetDirection = transform.forward;
        
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        
        transform.rotation = playerRotation;
    }

	public void HandleJump()
	{
        if (jumpRemaining > 0 && inputManager.jumpInput)
        {
            jumpRemaining--;

            playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0, playerRigidbody.linearVelocity.z);
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
            inputManager.jumpInput = false;
        }
        
	}

    private void CheckIsGrounded()
    {
        float maxDistance = 0.3f;

        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, maxDistance, groundLayerMask) &&
            playerRigidbody.linearVelocity.y <= 0.1f)
        {
            isGrounded = true;
            
        }
        else
        {
            isGrounded = false;
        }
        
        if (isGrounded)
        {
            ResetJump();
        }
    }

    private void ResetJump()
    {
        jumpRemaining = maxJumps;
    }
}
