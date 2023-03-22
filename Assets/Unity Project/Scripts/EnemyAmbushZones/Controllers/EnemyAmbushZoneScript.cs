using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Organizes data relevant to
/// </summary>
public class EnemyAmbushZoneScript : MonoBehaviour
{
    [SerializeField]
    private bool m_IsCompleted = false;

    public bool IsCompleted { get => m_IsCompleted; }

    private bool m_IsLocked { get => m_Lock ? m_Lock.LockIsActive : false; }

    public bool IsLocked { get => m_IsLocked; }

    private EnemyAmbushZoneLock m_Lock;
    private EnemyWaveController m_WaveController;
    public LevelManager LevelManager;

    private void Start()
    {
        m_Lock = GetComponent<EnemyAmbushZoneLock>();
        m_WaveController = GetComponent<EnemyWaveController>();
        LevelManager = LevelManager.Current;
    }

    // + + + + | Functions | + + + +

    public void OnLockActivated()
    {
        Debug.Log("EAZS - LOCK ACTIVATED BABY");

        // TODO: Wait in CRT here?

        // Start wave spawning
        m_WaveController.ExecuteCurrentWave();
    }

    public void OnWavesFinished()
    {
        Debug.Log("EAZS - WAVES FINISHED, Deactivating lock");
        m_IsCompleted = true;
        m_Lock.DeactivateLock();
    }
}