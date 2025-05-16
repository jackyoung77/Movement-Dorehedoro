using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
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

    private void Awake()
    {
        playerAnimManager = GetComponent<PlayerAnimManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
}

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            
            playerControls.PlayerMovement.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerMovement.Sprint.canceled += i => sprintInput = false;
            
            playerControls.PlayerMovement.Jump.started += i => jumpInput = true;
            playerControls.PlayerMovement.Jump.canceled += i => jumpInput = false;
		
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
    
}
