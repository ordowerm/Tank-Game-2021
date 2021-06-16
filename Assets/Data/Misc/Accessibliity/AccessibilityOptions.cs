using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * This stores some presets for accessibility options.
 * 
 * 
 */

[CreateAssetMenu(fileName = "Accessibility Settings",menuName = "Game Parameters/Accessibility Options")]
public class AccessibilityOptions : ScriptableObject
{
    [System.Serializable]
    public enum MagnifyTextMode
    {
        NORMAL, PERCENT_125, PERCENT_150, PERCENT_200,PERCENT_300
    }
    
    [System.Serializable]
    public enum ShaderStyle
    {
        FULL_SPECTRUM, //standard color set
        RG_LIMITED, //palettes designed for red-green color-deficient
        GRAYSCALE, //palettes designed for total color deficiency
        USE_PATTERNS //high-contrast settings that uses stuff like polka dots and stripes to distinguish enemy elements
    }

    public MagnifyTextMode magnify;
    public ShaderStyle shader;
}
