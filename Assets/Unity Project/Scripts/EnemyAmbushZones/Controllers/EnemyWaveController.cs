using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Works to generate EnemyWaves and work with EnemyAmbushZoneScript.
/// </summary>
[RequireComponent(typeof(EnemyAmbushZoneScript))]
public class EnemyWaveController : MonoBehaviour
{
    private int m_CurrentWaveIndex = 0;

    private List<EnemyWave> m_EnemyWaves = new(); // TODO: Load somewhere or generate?
    private List<Transform> m_SpawnTransforms = new();

    private EnemyAmbushZoneScript m_AmbushZone;
    public Transform SpawnParent;
    public List<EnemyAmbushZoneOnDeath> EnemyTypes = new(); // Types of enemies to spawn in the waves

    private void Awake()
    {
        m_AmbushZone = GetComponent<EnemyAmbushZoneScript>();

        // Automatically connect Spawn Transforms
        foreach (Transform tf in SpawnParent)
        {
            m_SpawnTransforms.Add(tf);
        }

        // Create Default Enemy Wave
        // TODO: RID OF THIS after I get better wave generation logic.
        EnemyWave defaultWave = new(this, 3);
        m_EnemyWaves.Add(defaultWave);
    }

    // + + + + | Functions | + + + +

    public bool CanExecuteWave(EnemyWave wave)
    {
        return !wave.IsWaveComplete;
    }

    public void ExecuteCurrentWave()
    {
        EnemyWave currentWave = m_EnemyWaves[m_CurrentWaveIndex];
        if (CanExecuteWave(currentWave))
        {
            for (int i = 0; i < currentWave.WaveSize; i++)
            {
                EnemyAmbushZoneOnDeath waveEnemy;
                if (currentWave.TryCreateWaveEnemy(out waveEnemy))
                {
                    // Pick random spawn point
                    var spawnIndex = Random.Range(0, m_SpawnTransforms.Count);

                    // Place instantiated object at position!
                    waveEnemy.transform.position = m_SpawnTransforms[spawnIndex].position;
                }
            }
        }
    }

    public void HandleWaveEnemyDeath(EnemyAmbushZoneOnDeath waveEnemy)
    {
        // Pass on the Enemy's Death to current wave's data
        EnemyWave currentWave = m_EnemyWaves[m_CurrentWaveIndex];
        currentWave.HandleWaveEnemyDeath(waveEnemy);

        // Check to see if current wave is over
        if (currentWave.IsWaveComplete)
        {
            // Do we have more waves to execute?
            if (m_CurrentWaveIndex + 1 < m_EnemyWaves.Count)
            {
                // TODO: Wait here in CRT?
                // Advance the Wave
                m_CurrentWaveIndex++;
                // Then Execute if possible
                ExecuteCurrentWave();
            }
            else
            {
                // Then we're good!
                m_AmbushZone.OnWavesFinished(); // TODO: Via event perhaps?
            }
        }
    }
}