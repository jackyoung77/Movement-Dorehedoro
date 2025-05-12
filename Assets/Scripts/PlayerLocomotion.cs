using System;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    Rigidbody playerRigidbody;
    
    private Vector3 moveDirection;
    private Transform cameraObject;

    public float movementSpeed = 7;
    public float rotationSpeed = 15;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.y = 0;
        moveDirection.Normalize();

        Vector3 movementVel = moveDirection * movementSpeed;
        playerRigidbody.linearVelocity = movementVel;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.y = 0;
        targetDirection.Normalize();
        
        if(targetDirection == Vector3.zero)
            targetDirection = transform.forward;
        
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        
        transform.rotation = playerRotation;
    }
}
