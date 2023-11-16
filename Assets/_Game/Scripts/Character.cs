using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ThrowWeapon throwWeapon;
    [SerializeField] public GameObject Weapon;
    [SerializeField] private float speed;

    protected string CurrentAnim;
    protected float range;
    protected bool isDead = false;
    private bool isMoving;

    public Animator Animator { get => animator; set => animator = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public float Speed { get => speed; set => speed = value; }

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
