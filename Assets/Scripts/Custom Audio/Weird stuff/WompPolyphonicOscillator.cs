using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WompPolyphonicOscillator : PolyphonicOscillator
{
    public PitchCurve pitchCurve;
    public override void PlayFrequency(float freq)
    {
        pitchCurve.StartNewPitchCurve(freq);
        base.PlayFrequency(freq);
    }

    protected void UpdateOffsetFrequencies()
    {

            oscillators[0].frequency_offset = pitchCurve.GetPitch();
        
    }

    private void Update()
    {
        if (freeOscillators.Count > 0)
        {
            UpdateOffsetFrequencies();
        }
    }
}
