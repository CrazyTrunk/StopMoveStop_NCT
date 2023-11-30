using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float range = 5f;
    private Vector3 startPosition;
    [SerializeField]private Rigidbody rb;
    protected Action<Character, Character> onHit;

    public Character shooter { get; set; }
    public virtual void OnInit(Character attacker, Action<Character, Character> onHit)
    {
        this.shooter = attacker;
        this.onHit = onHit;
    }

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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.CHARACTER))
        {
            Character victim = other.GetComponent<Character>();
            if(victim is not Player)
            {
                onHit?.Invoke(shooter, victim);

            }
        }
    }
}
