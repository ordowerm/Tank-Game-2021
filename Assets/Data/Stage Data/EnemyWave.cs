using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "GameData/Stage Data/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    public bool spawnOnEnterRegion; //set this to true if the enemies should spawn as soon as a certain region is entered
    public EnemySpawnData[] enemies;
}
