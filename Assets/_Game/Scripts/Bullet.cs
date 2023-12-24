using Lean.Pool;
using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    protected float range;
    protected Vector3 startPosition;
    protected Action<Character, Character> onHit;
    private Vector3 target;
    public Character Shooter { get; set; }
    public Vector3 Target { get => target; set => target = value; }
    protected bool isHitObstacle;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        isHitObstacle = false;
    }
    public void OnInit(Character attacker, Action<Character, Character> onHit)
    {
        this.Shooter = attacker;
        this.onHit = onHit;
        this.range = attacker.Range;
        transform.localScale *= attacker.ScaleMultiple;
        gameObject.AddComponent<CapsuleCollider>().isTrigger = true;
    }

    public virtual void Shoot()
    {
        startPosition = transform.position;
        Target = startPosition + (transform.forward * range);
        transform.localRotation = Quaternion.Euler(new Vector3(90f, 90f, 0f));
    }
    public virtual void Update()
    {
        if (!isHitObstacle)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target, range * 2f * Time.deltaTime);
            transform.Rotate(new Vector3(0f, 0f, 360f) * Time.deltaTime);
            if (Vector3.Distance(transform.position, Target) < 0.1f)
            {
                OnDespawn();
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.WALL) || other.CompareTag(Tag.OBSTACLE))
        {
            isHitObstacle = true;
            Invoke(nameof(OnDespawn), 2f);
        }
        if (other.CompareTag(Tag.CHARACTER))
        {
            Character victim = other.GetComponent<Character>();
            if (!victim.IsDead && victim != Shooter)
            {
                onHit?.Invoke(Shooter, victim);
                OnDespawn();
            }
        }
    }
    public void OnDespawn()
    {
        LeanPool.Despawn(gameObject);
    }
}
