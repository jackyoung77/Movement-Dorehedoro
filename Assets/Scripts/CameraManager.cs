using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform targetTransform;
    private Vector3 cameraFollowVel = Vector3.zero;

    public float cameraFollowSpeed = 0.2f;

    private void Awake()
    {
        targetTransform = FindObjectOfType<PlayerManager>().transform;
    }

    public void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVel, cameraFollowSpeed);
        transform.position = targetPosition;
    }
}
