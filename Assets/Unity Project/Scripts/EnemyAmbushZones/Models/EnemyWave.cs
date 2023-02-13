using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes a wave of enemies created by a EnemyWaveController
/// </summary>
public class EnemyWave
{
    public int WaveSize; // # of enemies in the wave
    public bool IsWaveComplete = false;

    public List<EnemyAmbushZoneOnDeath> m_LivingWaveEnemies;

    private EnemyWaveController m_WaveController;
    public EnemyWaveController WaveController { get => m_WaveController; }

    public EnemyWave(EnemyWaveController waveController, int waveSize)
    {
        m_WaveController = waveController;
        WaveSize = waveSize;
        m_LivingWaveEnemies = new();
    }

    // + + + + | Functions | + + + +

    public bool TryCreateWaveEnemy(out EnemyAmbushZoneOnDeath createdEnemy)
    {
        if (m_LivingWaveEnemies.Count < WaveSize)
        {
            createdEnemy = CreateWaveEnemy();
            return true;
        }
        else
        {
            createdEnemy = null;
            return false;
        }
    }

    private EnemyAmbushZoneOnDeath CreateWaveEnemy()
    {
        // TODO: Choose Random Enemy Type
        EnemyAmbushZoneOnDeath waveEnemy = Object.Instantiate(m_WaveController.EnemyTypes[Random.Range(0, m_WaveController.EnemyTypes.Count)]);
        waveEnemy.TrySetWaveController(m_WaveController);
        m_LivingWaveEnemies.Add(waveEnemy);
        return waveEnemy;
    }

    public void HandleWaveEnemyDeath(EnemyAmbushZoneOnDeath waveEnemy)
    {
        m_LivingWaveEnemies.Remove(waveEnemy);
        if (m_LivingWaveEnemies.Count <= 0)
        {
            IsWaveComplete = true;
        }
    }
}