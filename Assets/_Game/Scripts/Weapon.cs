using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Weapon : MonoBehaviour
{
    public float speed = 5f;
    public float range = 5f;
    private Vector3 startPosition;
    private Rigidbody rb;

    public Character Shooter { get; set; }

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
    public void Initialize(float scaleSize, Character shooter)
    {
        transform.localScale *=scaleSize;
        Shooter = shooter;  
    }
    void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag(Tag.ENEMY))
    //    {
    //        Enemy enemy = other.GetComponent<Enemy>();
    //        if (!enemy.IsDead && enemy != Shooter)
    //        {
    //            enemy.LastAttacker = Shooter;
    //            enemy.OnDeath();
    //            Destroy(gameObject);
    //            if (enemy.LastAttacker != null && !enemy.LastAttacker.IsDead)
    //            {
    //                enemy.LastAttacker.LevelUp(enemy.Level);
    //            }
    //        }

    //    }
    //}
}
