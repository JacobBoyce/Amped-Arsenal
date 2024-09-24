using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffects : MonoBehaviour
{
    [Header("Billboard Vars")]

    public bool wantBillboard;

    [Header("Bob Vars")]
    //bobbing values
    public bool wantBob;
    public bool islocal;
    public float floatStrength, speed, offset, deathDeleteTimer;
    private float timer;

    [Header("Shake Vars")]
    public bool islocalShake;
    public bool damaged;
    public float shakeStr, shakeSpeed, shakeDur;
    private float shakeOsc, shakeTimer;

    [Header("Pop up Vars")]
    public Rigidbody rbObj;
    public bool wantPopUp;
    public float delaytimeToSlowDown, popForce;
    private float delay;

    [Header("Flash Vars")]
    public SpriteRenderer thisSR;
    public bool wantFlash;
    public float blinkTimer, blinkDuration, blinkIntesity;

    Rigidbody rb;

    Vector2 floatY;
    Vector3 floatX;

    //switch bool
    bool isDead = false;

    //death vaues
    Vector3 deathPos;
    float angle;

    void Start ()
    {
        if(rbObj == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        else
        {
            rb = rbObj;
        }
        
        floatY = transform.localPosition;
        floatX = transform.localPosition;
        deathPos = new Vector3(-45, 0, 0);
        angle = transform.rotation.y;

        if(wantPopUp)
        {
            PopUp();
        }
    }

    void Update () 
    {
        //check if want to billboard
        if(wantBillboard)
        {
            transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z);
        }

        if(wantFlash)
        {
            if(blinkTimer > 0)
            {
                blinkTimer -= Time.deltaTime;
                float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
                float intensity = (lerp * blinkIntesity) + 1.0f;
                if(thisSR != null)
                {
                    thisSR.GetComponent<SpriteRenderer>().material.color = Color.white * intensity;
                }
            }
            else
            {
                blinkTimer = blinkDuration;
            }
        }

        if(!isDead && damaged)
        {
            if(shakeTimer < shakeDur)
            {
                shakeTimer += Time.deltaTime;
                shakeOsc += Time.deltaTime * shakeSpeed;
                floatX.x = Mathf.Abs(Mathf.Sin(shakeOsc) * shakeStr);
                if(islocalShake)
                {
                    transform.localPosition = new Vector3(floatX.x, transform.localPosition.y, 0);
                } else
                {
                    transform.position = new Vector3(floatX.x ,transform.position.y, transform.position.z);
                }
            }
            else
            {
                shakeTimer = 0;
                damaged = false;
            }
        }

        if(wantPopUp && wantBob)
        {
            if(delay < delaytimeToSlowDown)
            {
                delay += Time.deltaTime;
            }
            else
            {
                if(rb.velocity.magnitude > 0)
                {
                    rb.velocity = rb.velocity * 0.95f * Time.deltaTime;
                }
                Bobber();
            }
            //wait then bobber
        }
        else
        {
            if(!isDead && wantBob)
            {
                Bobber();
            }
            else if(isDead)
            {
                angle--;
                angle = Mathf.Clamp(angle, -45, 45);
                transform.localEulerAngles = new Vector3(angle, 0, 0);
                Destroy(this.gameObject, deathDeleteTimer);
                /*if(transform.localEulerAngles == deathPos)
                {
                    Destroy(this.gameObject);
                }*/
            }
        }
    }

    public void EndAnimation()
    {
        Destroy(this.gameObject);
    }

    public void Bobber()
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

    public void PopUp()
    {
        Vector3 dir = new Vector3(Random.Range(-2,2), popForce ,Random.Range(-2,2));

        rb.AddForce(dir, ForceMode.Impulse);
    }

    public void Died()
    {
        isDead = true;
    }
}
