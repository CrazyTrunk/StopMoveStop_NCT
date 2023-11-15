﻿using System;
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
    private float animSpeed = 1.5f;
    private void Awake()
    {
        character = GetComponentInParent<Character>();
        if (character is Player currentPlayer)
        {
            player = currentPlayer;
        }
        character.Animator.SetFloat("attackSpeed", animSpeed);
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
                    attackTime = clip.length / animSpeed;
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
            // Loại bỏ enemy ra khỏi hàng đợi
            enemyQueue = new Queue<Enemy>(enemyQueue.Where(e => e != enemy));
            enemy.OnEnemyKilled -= Enemy_OnEnemyKilled; // Loại bỏ listener

            // Ẩn detected circle nếu có
            DetectedCircle detectedCircle = other.GetComponentInChildren<DetectedCircle>();
            if (detectedCircle != null)
            {
                detectedCircle.Hide();
            }

            // Nếu enemy hiện tại rời khỏi, cần chọn target mới hoặc ngừng tấn công
            if (currentTargetEnemy == enemy)
            {
                if (enemyQueue.Count > 0)
                {
                    // Có enemy khác trong hàng đợi, chọn làm mục tiêu mới
                    CurrentTargetEnemy = enemyQueue.Peek();
                    DetectedCircle nextDetectedCircle = CurrentTargetEnemy.GetComponentInChildren<DetectedCircle>();
                    if (nextDetectedCircle != null)
                    {
                        nextDetectedCircle.Show();
                    }
                }
                else
                {
                    // Không còn enemy nào, cần ngừng tấn công
                    CurrentTargetEnemy = null;
                    IsAttacking = false;
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (CurrentTargetEnemy != null && !IsAttacking && !CurrentTargetEnemy.IsDead)
        {
            AttackCurrentEnemy();
        }
    }
    private void Enemy_OnEnemyKilled(Enemy enemy)
    {
        OnDestroyEnemy(enemy);
        enemyQueue = new Queue<Enemy>(enemyQueue.Where(e => !e.IsDead));
        UpdateTarget();
    }
    private void UpdateTarget()
    {
        if (enemyQueue.Count > 0)
        {
            currentTargetEnemy = enemyQueue.Peek();
            DetectedCircle detectedCircle = currentTargetEnemy.GetComponentInChildren<DetectedCircle>();
            if (detectedCircle != null)
            {
                detectedCircle.Show();
            }
        }
        else
        {
            currentTargetEnemy = null;
            IsAttacking = false;
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
        yield return new WaitForSeconds(0.5f);

        IsAttacking = false;
    }
}
