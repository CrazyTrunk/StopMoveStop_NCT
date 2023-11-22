using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ThrowWeapon throwWeapon;
    [SerializeField] public HandWeapon Weapon;
    [SerializeField] private float speed;
    [SerializeField] private float range;

    protected string CurrentAnim;
    protected bool isDead = false;
    private bool isMoving;
    private bool hasEnemyInSight;
    private bool isAttacking;
    public Animator Animator { get => animator; set => animator = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool HasEnemyInSight { get => hasEnemyInSight; set => hasEnemyInSight = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    public void ChangeAnim(string animName)
    {
        if (CurrentAnim != animName)
        {
            if (this is Player)
            {
                Debug.Log(animName);
            }

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
}
