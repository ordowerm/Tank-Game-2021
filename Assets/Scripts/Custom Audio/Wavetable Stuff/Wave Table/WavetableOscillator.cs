using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavetableOscillator : Oscillator
{
    public DiscreteWaveData wave;

    protected override float GetWaveValue(float p)
    {
        float normalized_phase = p / (2.0f * Mathf.PI);
        return wave.GetValue(normalized_phase);
    }
}
