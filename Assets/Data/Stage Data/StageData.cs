using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="StageData", menuName ="GameData/Stage Data/Stage Data")]
public class StageData : ScriptableObject
{
    [SerializeField]
    public StageRegion[] regions;
    public Texture2D[] bgs;
}



[Serializable]
public class EnemySpawnData
{
    public GameObject enemy; //reference to the prefab we're spawning
    public Vector2 spawnLocation; //local x-y offset, relative to the x,y coordinates of the StageRegion's Rect.
    public EnemyData edata;
    public EnemyStateMachine.Orientation orientation; //
}

[Serializable]
public class StageRegion
{
    public Rect reg; //define x, y location, and size, where region is located
    public EnemyWave[] waves;
    public Vector2[] playerSpawnLocations;
}