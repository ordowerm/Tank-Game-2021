using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MenuColorPalette",menuName="ColorData/MenuColorList")]
public class MenuColorList : ScriptableObject
{
    #if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "Enum Values, in order:  UNSELECTED, HIGHLIGHTED, SELECTED, DISABLED";
    #endif


    public Color[] textColors;
    public Color[] boxColors;


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

}
