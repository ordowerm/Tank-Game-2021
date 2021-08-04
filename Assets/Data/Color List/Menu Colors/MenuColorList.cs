using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MenuColorPalette",menuName="ColorData/MenuColorList")]
public class MenuColorList : ScriptableObject
{
    public Color[] textColors;
    public Color[] boxColors;
    public Color[] arrowColors;

    public Color whiteShade;
    public Color blackShade;
    public Color lightGrayShade;
    public Color darkGrayShade;
   
    public Color GetTextColor(MenuStatus s)
    {
        try
        {
            return textColors[(int)s];
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        return Color.white;
    }
    public Color GetBoxColor(MenuStatus s)
    {
        try
        {
            return boxColors[(int)s];
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        return Color.white;
    }
    public Color GetArrowColor(MenuStatus s)
    {
        try
        {
            return arrowColors[(int)s];
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        return Color.white;
    }

}
