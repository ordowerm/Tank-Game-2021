using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The asset created from this script should contain a list of colors, forming a color palette.
[CreateAssetMenu(fileName ="Color Set",menuName ="ColorData/ColorList")]
public class ColorList : ScriptableObject
{
    [TextArea(0, 10)]
    public string description;

    public Color[] colors;
    
    [System.Serializable]
    public struct KeyColorPair
    {
        public string tag;
        public Color color;
    }


    public KeyColorPair[] pairs;


    /*
     * Since Unity doesn't support Serializable generics, I can't easily store a Dictionary as an asset.
     * Accordingly, we just linearly go through the color/tag pairs and check if the string used for the tag is equal.
     * 
     * Runtime: Theta(n)
     *
     */
    public Color GetColorByTag(string t)
    {
        Color returnColor = Color.white;
        for (int i = 0; i<pairs.Length; i++)
        {
            if (pairs[i].tag.Equals(t))
            {
                returnColor = pairs[i].color;
                Debug.Log("color found: "+returnColor.ToString());
                break;
            }
        }


        return returnColor;
    }
    public Color GetColorByIndex(int i)
    {
        Color returnme = Color.white;
        if (i >= 0 && i < pairs.Length)
        {
            returnme = pairs[i].color;
        }

        return returnme;
    }
}
