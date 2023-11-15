using System;
using System.Collections;
using UnityEngine;

public class RandomPositionState : IState
{
    private Enemy _enemy;
    Vector3 direction;
    private float changeDirectionTime = 2f;
    private float timeSinceLastChange;

    public RandomPositionState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void OnEnter()
    {
        timeSinceLastChange = 0f;
        ChangeDirection();
    }

    private void ChangeDirection()
    {
        direction = UnityEngine.Random.onUnitSphere;
        direction.y = 0;
    }

    public void OnExecute()
    {
        _enemy.Move(direction);
        timeSinceLastChange += Time.deltaTime;
        if (timeSinceLastChange >= changeDirectionTime)
        {
            ChangeDirection();
            timeSinceLastChange = 0f;
        }
    }

    public void OnExit()
    {
    }
}