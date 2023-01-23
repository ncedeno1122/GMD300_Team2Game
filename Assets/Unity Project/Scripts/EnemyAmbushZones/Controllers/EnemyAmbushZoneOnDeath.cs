using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that allows a response
/// </summary>
public class EnemyAmbushZoneOnDeath : MonoBehaviour
{
    private Health m_HealthComponent;
    private EnemyWaveController m_EnemyWaveController; // The EnemyWaveController this belongs to.

    private void Awake()
    {
        m_HealthComponent = GetComponent<Health>();
    }

    private void OnEnable()
    {
        m_HealthComponent.OnDeath += HandleOnDeath;
        m_HealthComponent.OnHit += HandleOnHit;
    }

    private void OnDisable()
    {
        m_HealthComponent.OnDeath -= HandleOnDeath;
        m_HealthComponent.OnHit -= HandleOnHit;
    }

    // + + + + | Functions | + + + +

    /// <summary>
    /// Attempts to set the EnemyWaveController this belongs to, invoked upon instantiation.
    /// </summary>
    /// <param name="ewc"></param>
    /// <returns></returns>
    public bool TrySetWaveController(EnemyWaveController ewc)
    {
        if (m_EnemyWaveController == null)
        {
            m_EnemyWaveController = ewc;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void HandleOnDeath()
    {
        Debug.Log("Enemy defeated!");
    }

    private void HandleOnHit()
    {
        // If we've JUST been killed,
        if (m_HealthComponent.CurrentHealth <= 0)
        {
            m_EnemyWaveController.HandleWaveEnemyDeath(this);
        }
    }
}