using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    private bool canAttack;
    protected string CurrentAnim;
    protected float radius;
    protected bool IsDead = false;
    private bool hasEnemyInSight = false;

    public bool IsMoving;
    [SerializeField] private Animator animator;

    public bool CanAttack { get => canAttack; set => canAttack = value; }
    public bool HasEnemyInSight { get => hasEnemyInSight; set => hasEnemyInSight = value; }
    public Animator Animator { get => animator; set => animator = value; }

    public void ChangeAnim(string animName)
    {
        if (CurrentAnim != animName)
        {
            Animator.ResetTrigger(animName);
            CurrentAnim = animName;
            Animator.SetTrigger(CurrentAnim);
        }
    }
}
