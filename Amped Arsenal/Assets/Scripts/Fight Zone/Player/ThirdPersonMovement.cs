using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Joystick joystick;
    public CharacterController controller;
    public float speed = 6f, turnSmoothTIme = 0.1f;
    float turnSMoothVel;

    [Header("Gravity")]
    private float gravity = -9.81f;
    private Vector3 velocity;
    public Transform groundCheck;
    public float groundDist = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;
    public bool isMoving;

    // Update is called once per frame
    void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        isMoving = direction != Vector3.zero ? true : false;
        //Debug.Log("  " + isMoving);

        //gravity
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSMoothVel, turnSmoothTIme);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir =  Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}
