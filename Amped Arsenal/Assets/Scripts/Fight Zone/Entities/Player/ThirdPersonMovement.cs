using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    public Joystick joystick;
    public float speed;
    public Rigidbody theRB;
    public Vector2 moveInput;
    public Animator quispyAnim;
    public PlayerController player;

    public bool lookingLeft, lookingRight;

    public enum FlipState
    {
        WAIT,
        CHECKSTATE
    }
    public float flipThreshold, flipTimer;
    public FlipState fpState;

    public void Start()
    {
        fpState = FlipState.CHECKSTATE;
    }

    public void OnMovement(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        //moveInput.x = joystick.Horizontal;
        //moveInput.y = joystick.Vertical;
        //Debug.Log("doin stuff");
    }

    public void Update()
    {
        //moveInput.x = joystick.Horizontal;
        //moveInput.y = joystick.Vertical;
        
        

        //moveInput.x = Input.GetAxis("Horizontal");
        //moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        theRB.velocity = new Vector3(moveInput.x * player._stats["spd"].Value, 0f, moveInput.y * player._stats["spd"].Value);

        switch(fpState)
        {
            case FlipState.CHECKSTATE:
                //check direction about to go
                //if movedir.x < 0 == moving left
                if(moveInput.x < 0 && lookingLeft == false)
                {
                    flipTimer = 0;
                    fpState = FlipState.WAIT;
                    break;
                }
                //if looking right
                else
                {
                    //if movedir.x > 0 == moving right
                    if(moveInput.x > 0 && lookingRight == false)
                    {
                        flipTimer = 0;
                        fpState = FlipState.WAIT;
                        break;
                    }
                }
            break;

            case FlipState.WAIT:
                if(flipTimer < flipThreshold)
                {
                    flipTimer += Time.deltaTime;
                }
                else
                {
                    //check direction about to go
                    //if movedir.x > 0 == moving right
                    if(moveInput.x > 0 && lookingRight == false)
                    {
                        quispyAnim.SetBool("Flip",true);
                        lookingRight = true;
                        lookingLeft = false;
                        flipTimer = 0;
                        fpState = FlipState.WAIT;
                        break;
                    }
                    else if(moveInput.x < 0 && lookingLeft == false)
                    {
                        quispyAnim.SetBool("Flip",false);
                        lookingRight = false;
                        lookingLeft = true;
                        flipTimer = 0;
                        fpState = FlipState.WAIT;
                        break;
                    }
                    else
                    {
                        fpState = FlipState.CHECKSTATE;
                        break;
                    }
                }
            break;
        }
    }
}
