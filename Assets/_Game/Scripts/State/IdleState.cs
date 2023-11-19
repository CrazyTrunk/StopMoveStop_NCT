using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class IdleState : IState
{
    private EnemyController _enemy;
    private float idleTime = 1f;

    private float timeSinceLastChange;
    private float timeSinceLastChangeWithEnemy;

    public IdleState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void OnEnter()
    {
        timeSinceLastChange = 0f;
        _enemy.ChangeAnim(Anim.IDLE);
    }
    public void OnExecute()
    {
        if (_enemy.Enemy.HasEnemyInSight)
        {
            timeSinceLastChangeWithEnemy += Time.deltaTime;
            if (timeSinceLastChangeWithEnemy >= idleTime)
            {
                _enemy.Enemy.IsMoving = true;
                _enemy.SetState(new RandomPositionState(_enemy));

            }
        }
        else
        {
            timeSinceLastChange += Time.deltaTime;

            if (timeSinceLastChange >= idleTime)
            {
                _enemy.SetState(new RandomPositionState(_enemy));
            }
        }
    }

    public void OnExit()
    {
        timeSinceLastChange = 0f;
    }
}
