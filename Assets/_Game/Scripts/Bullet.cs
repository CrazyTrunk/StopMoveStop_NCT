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
        transform.localRotation = Quaternion.identity;
    }
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, 360f) * Time.deltaTime);
        if (Vector3.Distance(startPosition, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }
}
