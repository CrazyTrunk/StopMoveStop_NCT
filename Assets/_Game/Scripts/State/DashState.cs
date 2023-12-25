using UnityEngine;

public class DashState : IState
{
    private EnemyController _enemy;
    public DashState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExecute()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }
}
