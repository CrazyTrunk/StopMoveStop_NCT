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
    protected bool HasEnemyInSight = false;
    [SerializeField] private Animator animator;

    public bool CanAttack { get => canAttack; set => canAttack = value; }

    public void ChangeAnim(string animName)
    {
        if (CurrentAnim != animName)
        {
            animator.ResetTrigger(animName);
            CurrentAnim = animName;
            animator.SetTrigger(CurrentAnim);
        }
    }
}
