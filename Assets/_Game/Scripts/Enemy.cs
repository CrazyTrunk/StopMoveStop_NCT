using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : Character
{
    [SerializeField] private EnemyController controller;
    private float respawnTime = 1f;

    public void Move(Vector3 direction)
    {
        transform.position += Speed * Time.deltaTime * direction;
    }
    public void OnDeath()
    {
        OnEnemyDeath();
        controller.SetState(new IdleState(controller));
    }
    public void OnEnemyDeath()
    {
        IsDead = true;
        IsMoving = false;
        IsAttacking = false;
        HasEnemyInSight = false;
        capsuleCollider.enabled = false;
        Undetect();
        ChangeAnim(Anim.DIE);
        InvokeOnDeath();
        StartCoroutine(RespawnCoroutine());

    }
    private IEnumerator RespawnCoroutine()
    {

        // Đợi animation "die" chạy xong
        yield return new WaitForSeconds(AnimPlayTime / AnimSpeed);

        // Ẩn nhân vật (hoặc làm nhân vật không hoạt động) khi nó chết
        yield return new WaitForSeconds(respawnTime);
        LevelManager.Instance.BotKilled(this);

    }

}
