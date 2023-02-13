using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : MonoBehaviour
{
    public float FlyingSpeed = 3.5f;

    private FlyingEnemyState m_CurrentState;
    private Rigidbody2D m_Rigidbody;
    public Rigidbody2D Rigidbody { get => m_Rigidbody; }
    private LevelManager m_LevelManager;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_LevelManager = LevelManager.Current;
    }

    private void Start()
    {
        Transform playerTf = m_LevelManager.SceneCharacters[0].transform;
        ChangeState(new FlyingEnemyFollowWithOffset(this, playerTf, new Vector3(0f, 4f)));
    }

    private void Update()
    {
        m_CurrentState.OnUpdate();
    }

    private void FixedUpdate()
    {
        m_CurrentState.OnFixedUpdate();
    }

    // + + + + | Functions | + + + +

    private void ChangeState(FlyingEnemyState newState)
    {
        newState.Exit();
        m_CurrentState = newState;
        newState.Enter();
    }
}