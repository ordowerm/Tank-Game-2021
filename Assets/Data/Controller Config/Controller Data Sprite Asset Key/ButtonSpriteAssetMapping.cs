using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "Button Sprite Asset Mapping",menuName = "ScriptableObjects/Controller Map")]
public class ButtonSpriteAssetMapping : ScriptableObject
{
    public string inputName;
    public ControllerType controllerType;
    public KeyCode keycode;
    public string axisName;
    public string spriteAssetPrefix; //when generating the tag to use when doing rich-text embedding for sprites in TextMeshPro, use this prefix
    public TMP_SpriteAsset spriteAsset;
    public float frameDelay; //delay between advancing the textmesh
    public bool playReverseAnimation; //when looping animation, play in reverse after playing forward
}