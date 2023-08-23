using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithPlayer : MonoBehaviour
{
    public ThirdPersonMovement movementObj;
    public float turnSmoothTime = .2f;
    float turnSmoothVel;

    public void Start()
    {
        movementObj = GetComponentInParent<ThirdPersonMovement>();
    }
    public void Update()
    {
        float horizontal = movementObj.moveInput.x;
        float vertical = movementObj.moveInput.y;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //Vector3 moveDir =  Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}
