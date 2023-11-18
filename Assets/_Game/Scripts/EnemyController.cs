using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy currentBot;
    private IState currentState;

    public Enemy Enemy { get => currentBot; set => currentBot = value; }

    public void Start()
    {
       SetState(new IdleState(this));
    }
    private void Update()
    {
        currentState?.OnExecute();
    }
    public void SetState(IState newState)
    {
        currentState?.OnExit();

        currentState = newState;
        currentState?.OnEnter();
    }
    public void Move(Vector3 direction)
    {
        Enemy.Move(direction);
    }
    public void StopMovement()
    {
        Enemy.StopMovement();
    }
    public void ChangeAnim(string anim)
    {
        Enemy.ChangeAnim(anim);
    }
}
