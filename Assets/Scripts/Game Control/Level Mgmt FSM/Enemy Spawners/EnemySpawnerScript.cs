using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Script for spawning subwaves of enemies
 */
public class EnemySpawnerScript : MonoBehaviour
{
    public LevelManager mgmt; //reference to parent mgmt script
    protected int subwaveId; 
    public EnemyWave[] subwaves;


    private void Awake()
    {
        subwaveId = 0;
    }


    public void SpawnSubwave()
    {
        StartCoroutine(SubWaveSpawnCoroutine());

    }

    //Call from LevelManger when all registered enemies have been destroyed
    //If this returns true, then the LevelManager should advance to the next EnemySpawner.
    public bool NotifyLastDestroyed()
    {
        bool allSubwavesFinished = subwaveId >= subwaves.Length;
        if (!allSubwavesFinished)
        {
            if (subwaves[subwaveId].waitAllClear)
            {
                SpawnSubwave();
            }
        }
        return allSubwavesFinished;

    }


    protected IEnumerator SubWaveSpawnCoroutine()
    {
        yield return new WaitForSeconds(subwaves[subwaveId].spawnDelay);
        foreach(EnemySpawnData esd in subwaves[subwaveId].enemies)
        {
            mgmt.SpawnEnemy(esd);
        }
        subwaveId++;
        
        //Automatically start next subwave if it's not necessary to wait for the next wave
        if (subwaveId < subwaves.Length)
        {
            if (!subwaves[subwaveId].waitAllClear)
            {
                SpawnSubwave();
            }
        }
    }
}
