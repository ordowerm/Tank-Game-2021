using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bullet Data",menuName ="GameData/BulletData")]
public class BulletData :ScriptableObject
{
    public Sprite sprite;
    public Sprite trailParticleSprite;
    public Sprite hitParticleSprite;
    public ElementData element;
    public bool EnemyBullet; //flag for whether the bullet is for the player or for the enemy
    public float lifespan; //time before bullet disappears
    public float speed; //multiplier for when movement function is called
    public float ricochetTime; //lifespan  after ricochet (use for particle FX?)
    public float destroyTime; //lifespan after successful hit (use for particle FX)
    public int power; //amount of hp to deduct from enemy upon hit
}
