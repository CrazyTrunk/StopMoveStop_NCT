using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static System.TimeZoneInfo;

public class RaidalTrigger : MonoBehaviour
{
    private Queue<Enemy> enemyQueue = new Queue<Enemy>();
    private Enemy currentTargetEnemy = null;
    private Character character;
    private Player player;
    public Enemy CurrentTargetEnemy { get => currentTargetEnemy; set => currentTargetEnemy = value; }
    public bool IsAttacking;
    private float attackTime;

    private void Awake()
    {
        character = GetComponentInParent<Character>();
        if (character is Player currentPlayer)
        {
            player = currentPlayer;
        }
        UpdateAnimClipTimes();

    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = character.Animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Attack":
                    attackTime = clip.length;
                    Debug.Log($"attackTime {attackTime}");
                    break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (!enemyQueue.Contains(enemy))
            {
                enemy.OnEnemyKilled += Enemy_OnEnemyKilled;
                enemyQueue.Enqueue(enemy);
                if (CurrentTargetEnemy == null)
                {
                    //hien tai neu khong co targetnao => lay thang dau tien luon
                    CurrentTargetEnemy = enemyQueue.Peek();
                    DetectedCircle detectedCircle = other.GetComponentInChildren<DetectedCircle>();
                    if (detectedCircle != null)
                    {
                        detectedCircle.Show();
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemyQueue.Contains(enemy))
            {
                enemyQueue = new Queue<Enemy>(enemyQueue.Where(e => e != enemy));
                if(enemyQueue.Count == 0)
                {
                    DetectedCircle detectedCircle = other.GetComponentInChildren<DetectedCircle>();
                    if (detectedCircle != null)
                    {
                        detectedCircle.Hide();
                    }
                    CurrentTargetEnemy = null;
                }
               
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (CurrentTargetEnemy != null && !IsAttacking)
        {
            AttackCurrentEnemy();
        }
    }
    private void Enemy_OnEnemyKilled(Enemy enemy)
    {
        OnDestroyEnemy(enemy);
        enemyQueue = new Queue<Enemy>(enemyQueue.Where(e => e != enemy));
        currentTargetEnemy = null;
        if (enemyQueue.Count > 0)
        {
            currentTargetEnemy = enemyQueue.Peek();
            DetectedCircle detectedCircle = currentTargetEnemy.GetComponentInChildren<DetectedCircle>();
            if (detectedCircle != null)
            {
                detectedCircle.Show();
            }
        }
    }
    private void OnDestroyEnemy(Enemy enemy)
    {
        enemy.OnEnemyKilled -= Enemy_OnEnemyKilled;
    }

    private void AttackCurrentEnemy()
    {
        if (!player.IsMoving)
        {
            StartCoroutine(WaitForAnimation());
        }
    }
    IEnumerator WaitForAnimation()
    {
        IsAttacking = true;
        player.LookAtTarget(currentTargetEnemy);
        character.ChangeAnim("attack");
        yield return new WaitForSeconds(attackTime / 2);
        player.HideWeapon();
        if (character.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            player.ThrowWeapon();
        }
        else
        {
            player.ShowWeapon();
        }
        yield return new WaitForSeconds(attackTime / 2);
        player.ShowWeapon();

        if (!player.IsMoving)
        {
            character.ChangeAnim("idle");
        }
        yield return new WaitForSeconds(0.2f);

        IsAttacking = false;
    }
}
