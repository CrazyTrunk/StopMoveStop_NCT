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
        //Set Value First
        IsDead = true;
        StopAllCoroutines();
        StartCoroutine(WaitForAnimation());
    }
    IEnumerator WaitForAnimation()
    {
        ChangeAnim(Anim.DIE);

        Undetect();
        yield return new WaitForSeconds((AnimPlayTime / AnimSpeed));
        base.OnDeath();
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
        controller.Start();
    }
}
