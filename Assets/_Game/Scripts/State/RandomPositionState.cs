using UnityEngine;

public class RandomPositionState : IState
{
    private EnemyController _enemy;
    Vector3 direction;
    private float changeDirectionTime = 2f;
    private float timeSinceLastChange;

    private float randomChangeTime;

    private bool isRandomChangeDirection;
    private bool isChangeDirection;
    public RandomPositionState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public void OnEnter()
    {
        OnInit();
        DecideIfShouldChangeDirection();
        ChangeDirection();
        _enemy.transform.LookAt(_enemy.transform.position + direction);
    }

    private void DecideIfShouldChangeDirection()
    {
        isRandomChangeDirection = UnityEngine.Random.value > 0.5f;
        if (isRandomChangeDirection)
        {
            randomChangeTime = UnityEngine.Random.Range(0f, changeDirectionTime);
        }
    }

    private void ChangeDirection()
    {
        float angle = UnityEngine.Random.Range(0f, 360f);
        direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
    }

    public void OnExecute()
    {
        timeSinceLastChange += Time.deltaTime;
        _enemy.CurrentBot.IsMoving = true;
        _enemy.ChangeAnim(Anim.RUN);
        _enemy.Move(direction);
        if (_enemy.IsFacingWall(direction))
        {
            ChangeDirectionRandomlyDuringRun();
        }

        _enemy.transform.LookAt(_enemy.transform.position + direction);

        if (timeSinceLastChange >= randomChangeTime && isRandomChangeDirection && !isChangeDirection)
        {
            ChangeDirectionRandomlyDuringRun();
        }
        if (timeSinceLastChange >= changeDirectionTime)
        {
            timeSinceLastChange = 0f;
            _enemy.CurrentBot.IsMoving = false;
            _enemy.transform.LookAt(_enemy.transform.position + direction);
            if (!_enemy.CurrentBot.IsMoving)
            {
                _enemy.SetState(new IdleState(_enemy));
            }
        }

    }
    public void ChangeDirectionRandomlyDuringRun()
    {
        isChangeDirection = true;
        ChangeDirection();
        _enemy.transform.LookAt(_enemy.transform.position + direction);
    }
    public void OnExit()
    {
        OnInit();
    }
    private void OnInit()
    {
        _enemy.CurrentBot.IsMoving = false;
        isRandomChangeDirection = false;
        isChangeDirection = false;
        timeSinceLastChange = 0f;
        randomChangeTime = 0f;
    }
}