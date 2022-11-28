using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Joystick joystick;
    public float speed;
    public Rigidbody theRB;
    public Vector2 moveInput;
    public Animator quispyAnim;
    public SpriteRenderer thisSR;

    public enum FlipState
    {
        WAIT,
        CHECKSTATE
    }
    public float flipThreshold, flipTimer;
    public FlipState fpState;

    public void Flipperoo()
    {
        thisSR.flipX = thisSR.flipX == false ? true : false;
    }

    public void Start()
    {
        fpState = FlipState.CHECKSTATE;
    }

    public void Update()
    {
        //moveInput.x = joystick.Horizontal;
        //moveInput.y = joystick.Vertical;

        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        theRB.velocity = new Vector3(moveInput.x * speed, 0f, moveInput.y * speed);

        switch(fpState)
        {
            case FlipState.CHECKSTATE:
                if(thisSR.flipX == false)
                {
                    //check direction about to go
                    //if movedir.x < 0 == moving left
                    if(moveInput.x < 0)
                    {
                        flipTimer = 0;
                        fpState = FlipState.WAIT;
                        break;
                    }
                }
                //if looking right
                else
                {
                    //if movedir.x > 0 == moving right
                    if(moveInput.x > 0)
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
                    if(thisSR.flipX == false)
                    {
                        //check direction about to go
                        //if movedir.x > 0 == moving right
                        if(moveInput.x < 0)
                        {
                            quispyAnim.SetTrigger("Flip");
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
                    //if looking right
                    else
                    {
                        //if movedir.x < 0 == moving left
                        if(moveInput.x > 0)
                        {
                            quispyAnim.SetTrigger("Flip");
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
                }
            break;
        }
    }
}
