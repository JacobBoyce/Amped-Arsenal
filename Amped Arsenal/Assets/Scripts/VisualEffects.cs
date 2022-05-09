using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffects : MonoBehaviour
{
    
    //bobbing values
    public bool wantBob, islocal;
    public float floatStrength;
    public float speed;
    public float offset;
    private float timer;
    Vector2 floatY;

    //switch bool
    bool isDead = false;
    float deathSpd;

    //death vaues
    Vector3 deathPos;
    float angle;

    void Start ()
    {
        floatY = transform.localPosition;
        deathPos = new Vector3(-45, 0, 0);
        angle = transform.rotation.y;
    }

    void Update () 
    {
        if(!isDead && wantBob)
        {
            timer += Time.deltaTime * speed;
            floatY.y = offset + Mathf.Abs(Mathf.Sin(timer) * floatStrength);
            if(islocal)
            {
                transform.localPosition = new Vector3(0,floatY.y,0);
            } else
            {
                transform.position = new Vector3(transform.position.x ,floatY.y, transform.position.z);
            }
            
        }
        else if(isDead)
        {
            angle--;
            angle = Mathf.Clamp(angle, -45, 45);
            transform.localEulerAngles = new Vector3(angle, 0, 0);
            if(transform.localEulerAngles == deathPos)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Died()
    {
        isDead = true;
    }
}
