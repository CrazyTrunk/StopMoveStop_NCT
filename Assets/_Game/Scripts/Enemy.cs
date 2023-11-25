using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : Character
{
    [SerializeField] private EnemyController controller;

    public void Move(Vector3 direction)
    {
        transform.position += Speed * Time.deltaTime * direction;
    }
    public override void OnDeath()
    {
        base.OnDeath();

        controller.SetState(new IdleState(controller));
    }
}
