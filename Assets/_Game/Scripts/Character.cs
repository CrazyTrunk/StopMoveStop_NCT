using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour, ICombatant
{
    [SerializeField] private Animator animator;
    [SerializeField] private ThrowWeapon throwWeapon;
    [SerializeField] public HandWeapon Weapon;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private DetectedCircle detectedCircle;
    [SerializeField] private RadicalTrigger radicalTrigger;

    [SerializeField] private float animSpeed = 1.5f;
    [SerializeField] private float animPlayTime = 1f;
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider capsuleCollider;

    private float respawnTime = 1f;

    protected string CurrentAnim;
    private bool isMoving;
    private bool hasEnemyInSight;
    private bool isAttacking;


    public Animator Animator { get => animator; set => animator = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool HasEnemyInSight { get => hasEnemyInSight; set => hasEnemyInSight = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsDead { get; set; }
    public float AnimSpeed { get => animSpeed; set => animSpeed = value; }
    public float AnimPlayTime { get => animPlayTime; set => animPlayTime = value; }

    //Event
    public event Action<ICombatant> OnCombatantKilled;
    private void Awake()
    {
        Animator.speed = animSpeed;
        ResetState();
    }
    public void ChangeAnim(string animName)
    {
        if (CurrentAnim != animName)
        {
            Animator.ResetTrigger(animName);
            CurrentAnim = animName;
            Animator.SetTrigger(CurrentAnim);
        }
    }
    public void LookAtTarget(Transform target)
    {
        transform.LookAt(target.position);
    }
    public void ThrowWeapon()
    {
        throwWeapon.Throw(range);
    }
    public void HideWeaponOnHand()
    {
        Weapon.HideWeapon();
    }
    public void ShowWeaponOnHand()
    {
        Weapon.ShowWeapon();
    }

    public void Detect()
    {
        if(detectedCircle != null)
        detectedCircle.Show();
    }

    public void Undetect()
    {
        if (detectedCircle != null)
            detectedCircle.Hide();
    }

    public Transform GetTransform()
    {
        return transform;
    }
    public virtual void OnDeath()
    {
        IsDead = true;
        StopAllCoroutines();
        radicalTrigger.OnInit();
        Undetect();
        ChangeAnim("die");
        capsuleCollider.isTrigger = true;
        rb.isKinematic = true;
        StartCoroutine(RespawnCoroutine());
        OnCombatantKilled?.Invoke(this);
    }
    private IEnumerator RespawnCoroutine()
    {
        // Đợi animation "die" chạy xong
        yield return new WaitForSeconds(AnimPlayTime / animSpeed);

        // Ẩn nhân vật (hoặc làm nhân vật không hoạt động) khi nó chết
        yield return new WaitForSeconds(respawnTime);
        LevelManager.Instance.BotKilled(this);
        Respawn();
    }
    private void Respawn()
    {
        ResetState();
    }

    private void ResetState()
    {
        IsDead = false;
        IsMoving = false;
        IsAttacking = false;
        HasEnemyInSight = false;
        capsuleCollider.isTrigger = false;
        rb.isKinematic = false;
    }
}
