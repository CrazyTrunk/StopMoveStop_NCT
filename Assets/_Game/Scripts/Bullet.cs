using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float range = 5f;
    private Vector3 startPosition;
    [SerializeField]private Rigidbody rb;

    public Character Shooter { get; set; }

    public void Shoot()
    {
        startPosition = transform.position;
        rb.velocity = transform.forward * range;
    }
    void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }
}
