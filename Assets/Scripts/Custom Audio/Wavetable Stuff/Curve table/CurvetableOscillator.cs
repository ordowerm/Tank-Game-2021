using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvetableOscillator : Oscillator
{
    public AnimationCurve curve; //We use an animation curve that goes from domain [0,1] and range [-1,1] to define the wave
    public CurveList list;
    int listIndex=0;

    protected override float GetWaveValue(float p)
    {
        float normalized_phase = p / (2.0f * Mathf.PI);
        return curve.Evaluate(normalized_phase);
    }


    private void Awake()
    {
        curve = list.curves[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            listIndex = (listIndex + 1) % list.curves.Length;
            curve = list.curves[listIndex];
        }
    }
}
