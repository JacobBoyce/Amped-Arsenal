using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotParent : MonoBehaviour
{
    public float rotationSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0,rotationSpeed * Time.deltaTime,0));
    }
}
