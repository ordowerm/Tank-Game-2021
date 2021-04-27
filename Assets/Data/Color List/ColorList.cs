using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The asset created from this script should contain the color swatches for different skin colors.
[CreateAssetMenu(fileName ="Skin Color Set",menuName ="ScriptableObjects/ColorData/ColorList")]
public class ColorList : ScriptableObject
{
    public Color[] colors;
}
