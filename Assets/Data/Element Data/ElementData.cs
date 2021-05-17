using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Element Data",menuName ="GameData/ElementData")]
public class ElementData : ScriptableObject
{
    public int idNumber;
    public string elemName;
    public Sprite reticleSprite;
    public Color primary;
    public Color secondary;
}
