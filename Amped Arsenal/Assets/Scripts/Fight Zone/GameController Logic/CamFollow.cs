using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject followObj;
    public float smoothSpeed = 0.125f;
    private Vector3 velocity = Vector3.zero;
    public Vector3 posOffset;
    private Vector3 originalOffset;
    private Vector3 targetOffset = new(0, 30, -10);

    void Start()
    {
        // Set original offset (Y: 70, Z: -50, assuming X is 0)
        originalOffset = new Vector3(0, 70, -50);
        posOffset = originalOffset; // Initialize posOffset with the original values
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPos = followObj.transform.position + posOffset;
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);
        transform.position = smoothPos;

        this.transform.LookAt(followObj.transform); 
        
    }

    // Method to adjust posOffset to target values
    public void AdjustToTargetOffset(float duration)
    {
        StartCoroutine(AdjustOffset(posOffset, targetOffset, duration));
    }

    // Method to return posOffset to original values
    public void AdjustToOriginalOffset(float duration)
    {
        StartCoroutine(AdjustOffset(posOffset, originalOffset, duration));
    }

    // Coroutine to adjust posOffset smoothly over a given duration
    private IEnumerator AdjustOffset(Vector3 startOffset, Vector3 endOffset, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            posOffset = Vector3.Lerp(startOffset, endOffset, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final offset is set
        posOffset = endOffset;
    }
}
