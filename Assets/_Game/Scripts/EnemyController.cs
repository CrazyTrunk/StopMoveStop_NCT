using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    private IState currentState;

    public void Start()
    {
        SetState(new RandomPositionState(enemy));
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
 
}
