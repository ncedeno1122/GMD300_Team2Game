using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlyingEnemyState
{
    protected FlyingEnemyController m_FECContext;

    public FlyingEnemyState(FlyingEnemyController context)
    {
        m_FECContext = context;
    }

    public abstract void Enter();

    public abstract void Exit();

    public virtual void OnUpdate()
    { }

    public virtual void OnFixedUpdate()
    { }
}