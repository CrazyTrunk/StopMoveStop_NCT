using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    private bool canAttack;
    protected string CurrentAnim;
    protected float range;
    private bool isDead = false;
    public bool IsMoving;
    public float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private ThrowWeapon throwWeapon;
    [SerializeField] public GameObject Weapon;

    public bool CanAttack { get => canAttack; set => canAttack = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

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
        throwWeapon.Throw();
    }
    public void HideWeapon()
    {
        Weapon.SetActive(false);
    }
    public void ShowWeapon()
    {
        Weapon.SetActive(true);
    }
}
