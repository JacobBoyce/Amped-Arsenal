using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    Vector2 floatY;
    float originalY;

    public float floatStrength;
    public float speed;
    public float offset;
    private float timer;

    void Start ()
    {
        this.originalY = this.transform.localPosition.y;
        floatY = transform.localPosition;
    }

    void Update () {
        timer += Time.deltaTime * speed;
        floatY.y = offset + Mathf.Abs(Mathf.Sin(timer) * floatStrength);
        transform.localPosition = new Vector3(0,floatY.y,0);
    }
}
