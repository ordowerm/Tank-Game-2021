using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyData",menuName ="GameData/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public ElementData element;
    public int maxHp;
    public float iFrameTime; //time for invincibility between shots
    public float ricochetTime;
    public Rect walkColliderDimensions;
    public Rect[] hitboxColliderDimensions;
    public Animator animator;
    public Sprite baseSprite; //body sprite
    public int scoreForHit; //if enemy is hit, what do we update the player's score by?
    public int scoreForDestroyParticipation; //if the enemy is destroyed, every player deserves a participation trophy, I think. Maybe not.
    public int scoreForDestroyed; //if enemy is destroyed, what's it worth?

}
