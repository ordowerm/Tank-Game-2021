using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Curve List",menuName ="ScriptableObjects/AudioData/CurveList")]
public class CurveList : ScriptableObject
{
    public AnimationCurve[] curves;
}
