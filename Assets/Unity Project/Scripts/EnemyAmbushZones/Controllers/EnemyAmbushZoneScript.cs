using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Organizes data relevant to
/// </summary>
public class EnemyAmbushZoneScript : MonoBehaviour
{
    private bool m_IsCompleted = false;
    public bool IsCompleted { get => m_IsCompleted; }

    private bool m_IsLocked { get => m_Lock ? m_Lock.LockIsActive : false; }
    public bool IsLocked { get => m_IsLocked; }

    private EnemyAmbushZoneLock m_Lock;
    public LevelManager LevelManager;

    private void Awake()
    {
        m_Lock = GetComponent<EnemyAmbushZoneLock>();
    }

    // + + + + | Functions | + + + +

    public void OnLockActivated()
    {
        Debug.Log("LOCK ACTIVATED BABY");
    }
}