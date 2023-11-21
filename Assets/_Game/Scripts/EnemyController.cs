using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy currentBot;
    [SerializeField] private Transform infoCanvasTransform;
    private IState currentState;

    public Enemy CurrentBot { get => currentBot; set => currentBot = value; }

    public void Start()
    {
        SetState(new IdleState(this));
    }
    private void Update()
    {
        if (currentBot.IsDead)
        {
            return;
        }
        currentState?.OnExecute();
    }
    private void LateUpdate()
    {
        infoCanvasTransform.LookAt(infoCanvasTransform.position +
            Camera.main.transform.rotation * Vector3.forward, Vector3.up);
    }
    public void SetState(IState newState)
    {
        currentState?.OnExit();

        currentState = newState;
        currentState?.OnEnter();
    }
    public void Move(Vector3 direction)
    {
        CurrentBot.Move(direction);
    }
    public void ChangeAnim(string anim)
    {
        CurrentBot.ChangeAnim(anim);
    }
}
