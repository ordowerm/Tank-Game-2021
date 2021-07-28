using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "GameData/Stage Data/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    public bool spawnOnEnterRegion; //set this to true if the enemies should spawn as soon as a certain region is entered
    public bool waitAllClear; //if set to true, this means this wave should not be spawned if there are still enemies onscreen from previous wave
    public float spawnDelay; //delay before spawning this wave
    public EnemySpawnData[] enemies;
}
