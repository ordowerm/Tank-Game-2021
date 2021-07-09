using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Element Data",menuName ="GameData/ElementData")]
public class ElementData : ScriptableObject
{
    public Element element; //enumeration for this element type
    public int idNumber; //id number for this element --> currently in the process of refactoring this so that we use an enum instead
    public string elemName; //name of this element --> currently in the process of refactoring this so that we use an enum instead
    public Sprite reticleSprite; //sprite to use for a player reticle aiming with a weapon using this element
    public Sprite reticleOutlineSprite;
    public Color primary; //primary color to match element
    public Color secondary; //secondary color to match element
    public Color outlineColor; //color for outline
    public Texture2D elemTexture; //texture to apply to shader when using accessibility mode
    public float textureScale; //scale factor for when we map the elemTexture to the UVs when using accessibility mode
    public Sprite explosionParticleSprite;
}
