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
    public SpriteRenderer theSR;
    public bool lookedLeft = false, lookedRight = false;

    public void Flipperoo()
    {
        if(theSR.flipX == true)
        {
            theSR.flipX = false;
        }
        else if(theSR.flipX == false)
        {
            theSR.flipX = true;
        }
    }

    public void Update()
    {
        moveInput.x = joystick.Horizontal;
        moveInput.y = joystick.Vertical;
        moveInput.Normalize();

        theRB.velocity = new Vector3(moveInput.x * speed, 0f, moveInput.y * speed);

        if(moveInput.x < 0)
        {
            if(lookedLeft == false)
            {                
                lookedLeft = true;
                lookedRight = false;
                if(theSR.flipX == false)
                {
                    quispyAnim.SetTrigger("Flip");
                }
            }
            
        } else if(moveInput.x > 0)
        {
            if(lookedRight == false)
            {
                lookedLeft = false;
                lookedRight = true;
                if(theSR.flipX == true)
                {
                    quispyAnim.SetTrigger("Flip");
                }
            }
        }
    }
}
