using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParallaxData
{
    public Sprite sprite; //texture to use for parallax layer
    public Color spriteTint; //tint
    public SpriteDrawMode drawMode; 
    public Vector2 position; //start position of bg layer
    public float depth; //depth multiplier. closer objects should move faster
    public Vector2 size;
    public Vector2 baseSpeeds; //default translation speed, irrespective of depth value
}


[CreateAssetMenu(fileName ="BG Image List",menuName ="ScriptableObjects/BGImageList")]
//
public class ParallaxLayers : ScriptableObject
{
    public ParallaxData[] layers;
    public float GetTotalHeight()
    {
        float result = 0;
        foreach (ParallaxData d in layers)
        {
            result += d.size.y;
        }
        return result;
    }
}
