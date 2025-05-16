using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    Sliding sliding;
    PlayerAnimManager playerAnimManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;
    
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool sprintInput;
    public bool jumpInput;
    public bool slideInput;

    public bool startSliding;

    private void Awake()
    {
        playerAnimManager = GetComponent<PlayerAnimManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        sliding = GetComponent<Sliding>();
}

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            
            playerControls.PlayerMovement.Sprint.started += i => sprintInput = true;
            playerControls.PlayerMovement.Sprint.canceled += i => sprintInput = false;
            
            playerControls.PlayerMovement.Jump.started += i => jumpInput = true;
            playerControls.PlayerMovement.Jump.canceled += i => jumpInput = false;
            
            playerControls.PlayerMovement.Slide.started += i => startSliding = true;
            playerControls.PlayerMovement.Slide.started += i => slideInput = true;
            playerControls.PlayerMovement.Slide.canceled += i => slideInput = false;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintInput();
        HandleSlideInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        
        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;
        
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        playerAnimManager.UpdateAnimatorValues(0,moveAmount, playerLocomotion.isSprinting);
    }

    private void HandleSprintInput()
    {
        if (sprintInput && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleSlideInput()
    {
        if (startSliding && playerLocomotion.isGrounded && moveAmount > 0.5f)
            sliding.StartSlide();
        
        if (slideInput && sliding.slideTimer >= 0)
        {
            playerLocomotion.isSliding = true;
        }
        else
        {
            playerLocomotion.isSliding = false;
        }
    }
    
}
