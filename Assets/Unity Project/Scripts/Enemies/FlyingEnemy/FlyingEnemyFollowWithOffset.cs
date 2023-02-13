using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyFollowWithOffset : FlyingEnemyState
{
    private Vector3 m_Offset = new();
    private Transform m_Target;
    private Rigidbody2D m_Rb2d;
    public const float ACCEPTABLE_DISTANCE = 0.5f;

    public FlyingEnemyFollowWithOffset(FlyingEnemyController context, Transform target, Vector3 offset) : base(context)
    {
        m_Offset = offset;
        m_Target = target;
        m_Rb2d = context.Rigidbody;
    }

    public override void Enter()
    {
        //
    }

    public override void Exit()
    {
        //
    }

    public override void OnUpdate()
    {
        Vector3 targetPosition = (m_Target.position) + m_Offset;
        Vector3 steeringDirection = (targetPosition - (Vector3)m_Rb2d.position).normalized;

        if (Vector3.Distance(targetPosition, (Vector3)m_Rb2d.position) > ACCEPTABLE_DISTANCE)
        {
            m_Rb2d.MovePosition(m_Rb2d.position + ((Vector2)steeringDirection * (m_FECContext.FlyingSpeed * Time.fixedDeltaTime)));
            //m_Rb2d.velocity = (Vector2)steeringDirection * (m_FECContext.FlyingSpeed * Time.fixedTime);
        }
    }
}