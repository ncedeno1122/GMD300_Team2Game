using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyLaunchProjectiles : MonoBehaviour
{
    private float m_DeltaTimeHelper;
    public float FireRate;

    public Rigidbody2D ProjectileType;
    private IEnumerator m_FiringCRT;

    private void Awake()
    {
        m_FiringCRT = FiringCRT();
    }

    private void Start()
    {
        StartCoroutine(m_FiringCRT);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public IEnumerator FiringCRT()
    {
        while (true)
        {
            if (m_DeltaTimeHelper >= FireRate)
            {
                var projectile = Instantiate(ProjectileType);
                projectile.position = transform.position;
                m_DeltaTimeHelper = 0f;
            }

            m_DeltaTimeHelper += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}