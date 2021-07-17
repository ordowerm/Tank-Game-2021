using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TMP_String_Combiner : MonoBehaviour
{
    float delay; //Delay between calls to coroutine
    public TMP_Text text;
    public TMP_Animate_Sprite[] sprites;

    //This structure should contain either a string or a reference to whatever TMP_Animate_Sprite index is to be used. The coroutine for this script concatenates all of these items.
    [System.Serializable]
    public struct TMP_String_Mixed
    {
        /*
         Notes on the field int paramId:
         This field denotes the index in the sprite array TMP_Animate_Sprite[] corresponding to the sprite that should be rendered inline.
         If paramId equals -1, let that denote "don't display a sprite; display m_string instead.
         If paramId equals -2, let that denote '\n', since you can't include escape characters in the UnityEditor Inspector.
         */
        public int paramId; 
        public string m_string;

    }

    public TMP_String_Mixed[] strings; //list of strings to concatenate

    string GetConcatenatedString()
    {
        string result = "";

        //iterate each TMP_String_Mixed and add corresponding message
        foreach (TMP_String_Mixed s in strings)
        {
            if (
                s.paramId >= 0 &&
                s.paramId < sprites.Length
                )
            {
                result += sprites[s.paramId].GetSpriteAssetTag();
            }
            
            //-1 denotes regular text
            else if (s.paramId ==-1)
            {
                result += s.m_string;
            }

            //-2 denotes line break
            else if (s.paramId ==-2)
            {
                result += "\n";
            }

            //notice: if an invalid paramId is used, nothing gets appended to the result string.
        }


        return result;
    }


    private void Awake()
    {
        //If there are TMP_Animate_Sprites attached, use the fastest-updating one as the delay between calls to the coroutine
        delay = 1f; //default value
        if (sprites.Length > 0)
        {
            delay = sprites[0].key.frameDelay;
            foreach (TMP_Animate_Sprite s in sprites)
            {
                if (s.key.frameDelay < delay)
                {
                    delay = s.key.frameDelay; //minimizes delay time
                }
               
            }


            //Start coroutine.
            StartCoroutine(UpdateText());
        }
        else
        {

            //Onetime call to update the TMP_Text object if no animated elements present
            text.text = GetConcatenatedString();
        }

    }

    //Coroutine for updating the string contained in the TMP_Text
    IEnumerator UpdateText()
    {
        for(; ; )
        {
            text.text = GetConcatenatedString();
            Canvas.ForceUpdateCanvases();
            yield return new WaitForSeconds(delay);
        }
    }
}
