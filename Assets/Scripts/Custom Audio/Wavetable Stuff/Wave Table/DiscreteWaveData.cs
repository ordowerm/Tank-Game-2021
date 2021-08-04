using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Discrete Wave Data",menuName ="ScriptableObjects/AudioData/DiscreteWaveData")]
public class DiscreteWaveData : ScriptableObject
{
    public float[] values; //
    public float GetValue(float time)
    {
        float val = time;
        if (val < 0) { val = 0; }
        if (val > 1) { val = 1; }
        return values[ Mathf.RoundToInt(val * ((float)values.Length-1))];
    }

}
