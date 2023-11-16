using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class IdleState : IState
{
    private EnemyController _enemy;
    private float idleTime = 2f;
    private float timeSinceLastChange;

    public IdleState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void OnEnter()
    {
        timeSinceLastChange = 0f;
        _enemy.ChangeAnim("idle");
    }
    public void OnExecute()
    {
        timeSinceLastChange += Time.deltaTime;
        if (timeSinceLastChange >= idleTime)
        {
            _enemy.SetState(new RandomPositionState(_enemy));
        }
    }

    public void OnExit()
    {
        timeSinceLastChange = 0f;
    }
}
