using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class Bullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    protected float attackSpeed = 2f;
    protected float range;
    protected Vector3 startPosition;
    protected Action<Character, Character> onHit;

    public Character Shooter { get; set; }
    public void OnInit(Character attacker, Action<Character, Character> onHit)
    {
        this.Shooter = attacker;
        this.onHit = onHit;
        this.range = attacker.Range;
        transform.localScale *= attacker.ScaleMultiple;
        gameObject.AddComponent<BoxCollider>().isTrigger = true;

    }

    public virtual void Shoot()
    {
        startPosition = transform.position;
        rb.velocity = attackSpeed * range * transform.forward;
        transform.localRotation = Quaternion.identity;
    }
    public virtual void Update()
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
            if (!victim.IsDead && victim != Shooter)
            {
                onHit?.Invoke(Shooter, victim);
                Destroy(gameObject);
            }
        }
    }
}
