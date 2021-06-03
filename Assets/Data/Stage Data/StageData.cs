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
    public GameObject enemy;
    public Vector2 spawnLocation;
    public EnemyData edata;
}

[CreateAssetMenu(fileName = "StageData", menuName = "GameData/Stage Data/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    public bool spawnOnEnterRegion;
    public EnemySpawnData[] enemies;
}

[Serializable]
public class StageRegion
{
    public Rect reg; //define x, y location, and size, where region is located
    public EnemyWave[] waves;
}