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
    private Player player;
    public Enemy CurrentTargetEnemy { get => currentTargetEnemy; set => currentTargetEnemy = value; }

    private void Awake()
    {
        character = GetComponentInParent<Character>();
        if(character is Player currentPlayer)
        {
            player = currentPlayer;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (other.CompareTag("Enemy"))
        {
            enemyQueue.Enqueue(enemy);
            player.HasEnemyInSight = true;
            DetectedCircle detectedCircle = other.GetComponentInChildren<DetectedCircle>();
            if (detectedCircle != null)
            {
                detectedCircle.Show();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            AttackCurrentEnemy(other);
        }
    }
    private void AttackCurrentEnemy(Collider collider)
    {
        if (enemyQueue.Count > 0)
        {
            CurrentTargetEnemy = enemyQueue.Peek();
        }
    }

}
