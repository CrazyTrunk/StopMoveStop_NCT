using System;
using UnityEditor.Tilemaps;

public class IdleState : IState
{
    private Enemy _enemy;
    public IdleState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void OnEnter()
    {
        _enemy.ChangeAnim("idle");
    }

    public void OnExecute()
    {
    }

    public void OnExit()
    {
    }
}
