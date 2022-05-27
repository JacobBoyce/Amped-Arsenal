using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBG : MonoBehaviour
{
    [Range(-2f,2f)]
    public float scrollXSpeed = 0.5f;
    [Range(-2f,2f)]
    public float scrollYSpeed = 0.5f;
    private float offsetX, offsetY;
    public Material mat;

    // Update is called once per frame
    void Update()
    {
        offsetX += (Time.deltaTime * scrollXSpeed) / 10f;
        offsetY += (Time.deltaTime * scrollYSpeed) / 10f;
        mat.SetTextureOffset("BG", new Vector2(offsetX, offsetY));
    }
}
