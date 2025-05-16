using System;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;
    InputManager inputManager;

    public float maxSlideTime = 0.75f;
    public float slideForce = 200f;
    public float slideTimer;

    public bool slideStarted;

    private void Awake()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
        inputManager = GetComponent<InputManager>();
    }

    public void StartSlide()
    {
        //start animations
        playerLocomotion.playerRigidbody.AddForce(-Vector3.up * 5f, ForceMode.Impulse);
        slideTimer = maxSlideTime;
        inputManager.startSliding = false;
        slideStarted = true;
    }

    public void SlidingLocomotion()
    {
        if (!playerLocomotion.onSlope || playerLocomotion.playerRigidbody.linearVelocity.y > -0.1f)
        {
            playerLocomotion.playerRigidbody.AddForce(playerLocomotion.moveDirection * slideForce, ForceMode.Impulse);
            slideTimer -= Time.deltaTime;
        }
        else
        {
            playerLocomotion.playerRigidbody.AddForce(playerLocomotion.GetSlopeMoveDirection(playerLocomotion.moveDirection) * slideForce, ForceMode.Impulse);
        }
        
        
        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        playerLocomotion.isSliding = false;
        slideStarted = false;
        //stop animations
    }
}
