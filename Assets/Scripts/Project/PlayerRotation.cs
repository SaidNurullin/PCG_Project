using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float sensitivity = 5f; // Mouse sensitivity

    private float xRotation = 0f; // Rotation around the x-axis (up/down)
    private float yRotation = 0f; // Rotation around the y-axis (left/right)

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Rotate the player around the y-axis (left/right)
        yRotation += mouseX;
        transform.eulerAngles = new Vector3(0f, yRotation, 0f);

        // Rotate the camera around the x-axis (up/down), with limits
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit up/down rotation
        Camera.main.transform.eulerAngles = new Vector3(xRotation, yRotation, 0f);
    }
}