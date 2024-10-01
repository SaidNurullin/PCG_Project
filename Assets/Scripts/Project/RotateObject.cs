using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private string targetTag;
    [SerializeField] private float rotationAngle;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse click
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)), out hit))
            {
                if (hit.collider.gameObject.CompareTag(targetTag)) // Check if clicked object has the correct tag
                {
                    hit.collider.gameObject.transform.Rotate(0f, rotationAngle, 0f); // Rotate the object
                }
            }
        }
    }
}
