using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/*
 Script to assist in the rendering of TextMeshPro sprite meshes.
 
 */
public class TMP_Animate_Sprite : MonoBehaviour
{
    public ButtonSpriteAssetMapping key; //Sprite assets to poll from
    int spriteFrameId;

    private void Awake()
    {
        spriteFrameId = 0;
       // GetComponent<TMP_SubMesh>().spriteAsset = key.spriteAsset;
        StartCoroutine(UpdateSpriteIndex());
        /*foreach (ButtonSpriteAssetMapping b in keys)
        {
            Debug.Log("In animate sprite"+b.spriteAsset.spriteGlyphTable.Count);
        }*/
    }


    //Returns rich text tag corresponding to the correct sprite mesh
    public string GetSpriteAssetTag()
    {
        string result = "";
        result += "<sprite name=\""
                   + key.spriteAssetPrefix
                   + "\" index="
                   + spriteFrameId
                   + ">";
        

        return result;
    }

    
    //Takes in the index of the sprite asset whose sprite number needs updating
    IEnumerator UpdateSpriteIndex()
    {
        bool forward = true;
        for (; ; )
        {
            yield return new WaitForSeconds(key.frameDelay);
            int framenumber = spriteFrameId;
            if (forward)
            {
                framenumber++;
                if (!key.playReverseAnimation)
                {
                    framenumber %= key.spriteAsset.spriteGlyphTable.Count;
                }
                else
                {
                    if (framenumber >= key.spriteAsset.spriteGlyphTable.Count-1)
                    {
                        forward = false;
                    }
                }
            }
            else
            {
                if (framenumber <= 0) { 
                    forward = true; 
                }
                else
                {
                    framenumber--;
                }
            }
            spriteFrameId = framenumber;
        }
    }


    private void OnDestroy()
    {
        StopCoroutine(this.UpdateSpriteIndex());
    }
}
