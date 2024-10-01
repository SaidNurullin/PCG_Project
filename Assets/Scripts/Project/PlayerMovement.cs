using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    private float currentSpeed;
    public float jumpHeight = 10f;
    public float verticalVelocity;

    private bool isJumping = false;
    private bool isCrouching = false;

    private CharacterController controller;
    public float gravity = -9.8f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = speed;
        verticalVelocity = gravity;
    }

    // Update is called once per frame
    void Update()
    {
        // double speed
        bool doubleSpeed = Input.GetKey(KeyCode.LeftShift);
        if (doubleSpeed)
        {
            currentSpeed = speed * 2;
        }
        else
        {
            currentSpeed = speed;
        }

        // change velocity by gravity
        if (verticalVelocity > gravity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // perform jumping
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isCrouching)
        {
            Jump(jumpHeight);
        }

        // perform crounching
        if (Input.GetKey(KeyCode.C) && !isJumping)
        {
            Crouch();
        }


        // move the character
        var horizontalAxis = Input.GetAxis("Horizontal") * currentSpeed;
        var verticalAxis = Input.GetAxis("Vertical") * currentSpeed;



        var movement = new Vector3(horizontalAxis, verticalVelocity, verticalAxis) * Time.deltaTime;

        movement = transform.TransformDirection(movement);

        controller.Move(movement);


        // remove crounching effects
        if (!Input.GetKey(KeyCode.C))
        {
            isCrouching = false;
            controller.height = 2f;
        }

        // stop jumping
        if (controller.isGrounded)
        {
            isJumping = false;
        }


    }

    public void Jump(float height)
    {
        verticalVelocity = Mathf.Sqrt(height * -2f * gravity);
        isJumping = true;
    }

    private void Crouch()
    {
        currentSpeed = speed / 2;
        controller.height = 1.4f;
        isCrouching = true;
    }
}
