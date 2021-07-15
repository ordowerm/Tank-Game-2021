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
        public int paramId; //set to -1 to denote a regular string. Otherwise, this denotes the TMP_Animate_Sprite to use for drawing.
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
            else
            {
                result += s.m_string;
            }
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
                //GameObject newGameObject = new GameObject();
                //newGameObject.transform.SetParent(this.transform);
                //TMP_SubMesh submesh = newGameObject.AddComponent<TMP_SubMesh>();
                //submesh.spriteAsset.
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
            yield return new WaitForSeconds(delay);
        }
    }
}
