using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOscillator : MonoBehaviour
{
    public Light glowLight;

    public float intensityAmount, speed;
    public float minIntensity, maxIntensity;
    // Start is called before the first frame update
    void Start()
    {
        glowLight = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        float time = Mathf.PingPong(Time.time * speed, intensityAmount);
        //Debug.Log(time);
        glowLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, time);
        //glowLight.intensity = Mathf.PingPong(intensityAmount,duration); //Mathf.Abs(Mathf.Sin(intensityAmount)) * duration;
    }
}
