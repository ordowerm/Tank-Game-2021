using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 When rendering button layouts, etc., in a TextMeshPro text field, there needs to be some way of associating a button input to an associated
 TMP Sprite Asset, so that the correct button displays.
 
 This ScriptableObject maps the input to the appropriate sprite asset
 */
[CreateAssetMenu(fileName ="Button to Sprite Asset List",menuName ="ScriptableObjects/Controller Map List")]
public class ControllerSpriteAssetKeyMap : ScriptableObject
{
    public ButtonSpriteAssetMapping[] inputPairs;
}

public enum ControllerType
{
    Xbox, PS, Keyboard, Nintendo, Misc
}
