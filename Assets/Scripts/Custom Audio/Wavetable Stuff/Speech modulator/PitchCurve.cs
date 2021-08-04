using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 When imitating speech, you can achieve some fun sounds by using an AnimationCurve to interpolate the base_frequency of the oscillator.
 
 */
public class PitchCurve : MonoBehaviour
{
    public AnimationCurve[] curves;
    public float pitchRange; //pitch range, in cents
    public float maxTime; //duration of pitch modulation
    protected double sampling_frequency = 48000.0;

    //Runtime parameters
    AnimationCurve currentCurve;
    double timer = 0;
    double increment;
    float startPitch; //in Hertz
    float endPitch;


    private void Awake()
    {
        currentCurve = curves[Random.Range(0, curves.Length)];

    }

    public void StartNewPitchCurve(float baseFrequency)
    {
        float centModifier = Random.Range(-pitchRange, pitchRange);
        startPitch = baseFrequency * Mathf.Pow(2, centModifier / 1200f);
        centModifier = Random.Range(-pitchRange, pitchRange);
        endPitch = baseFrequency * Mathf.Pow(2, centModifier / 1200f);
        timer = 0;
        currentCurve = curves[Random.Range(0, curves.Length)];
        increment = maxTime / sampling_frequency / Time.deltaTime;
    }

     public float GetPitch()
    {
        float result = startPitch + (endPitch - startPitch) * currentCurve.Evaluate((float)timer);
        if (timer < 1)
        {
            timer += increment;
            if (timer > 1) { timer = 1; }
        }
        Debug.Log(result);
        return result;
    }
}
