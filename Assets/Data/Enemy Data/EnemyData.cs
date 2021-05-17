using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyData",menuName ="Game Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public ElementData element;
    public float maxHp;
    public float iFrameTime; //time for invincibility between shots

}
