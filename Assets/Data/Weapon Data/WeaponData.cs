using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Weapon Data",menuName ="GameData/WeaponData")]
public class WeaponData : ScriptableObject
{
    public Sprite weaponsprite;
    public Color weaponcolor;
    public bool isAutomatic;
    public float firingDelay;
    public BulletData bullettype;
    public bool chargeable;
    public float chargetime;
    public Vector2 spriteOffset; //position at which bullets should spawn
}
