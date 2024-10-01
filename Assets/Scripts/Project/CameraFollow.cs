using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public Vector3 offset; // Offset from the player

    void LateUpdate()
    {
        // Calculate the target position for the camera
        Vector3 desiredPosition = playerTransform.position + offset;

        // Set the camera's position
        transform.position = desiredPosition;
    }
}
