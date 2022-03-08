using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject followObj;
    public float smoothSpeed = 0.125f;
    private Vector3 velocity = Vector3.zero;
    public Vector3 posOffset;


    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPos = followObj.transform.position + posOffset;
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);
        transform.position = smoothPos;

        this.transform.LookAt(followObj.transform); 
        
    }
}
