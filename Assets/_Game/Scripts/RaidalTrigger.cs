using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaidalTrigger : MonoBehaviour
{
    private Queue<Enemy> enemyQueue = new Queue<Enemy>();
    private Enemy currentTargetEnemy = null;
    private Character character;
    private bool isAttacking = false;
    private void Awake()
    {
        character = GetComponentInParent<Character>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (other.CompareTag("Enemy"))
        {
            enemyQueue.Enqueue(enemy);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && !isAttacking)
        {
            AttackNextEnemy(other);
        }
    }
    private void AttackNextEnemy(Collider collider)
    {
        if (enemyQueue.Count > 0)
        {
            currentTargetEnemy = enemyQueue.Peek();

            DetectedCircle detectedCircle = collider.GetComponentInChildren<DetectedCircle>();
            if (detectedCircle != null)
            {
                detectedCircle.Show();
            }
            StartCoroutine(AttackEnemy(currentTargetEnemy));
        }
    }
    private IEnumerator AttackEnemy(Enemy enemy)
    {
        isAttacking = true;
        yield return new WaitForSeconds(1f);
        Player player = character as Player;
        Enemy currentEnemy = enemy.GetComponent<Enemy>();
        if (player.CanAttack)
        {
            player.Throw(currentEnemy);
        }
        isAttacking = false;
    }
}
