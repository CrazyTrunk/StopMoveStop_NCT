using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RadicalTrigger : MonoBehaviour
{
    private Queue<ICombatant> combatantQueue = new Queue<ICombatant>();
    private ICombatant currentTarget = null;
    private Character character;
    public bool IsAttacking;
    private float attackTime;
    private float animSpeed = 1.5f;

    public ICombatant CurrentTarget { get => currentTarget; set => currentTarget = value; }

    private void Awake()
    {
        character = GetComponentInParent<Character>();
        character.Animator.SetFloat("attackSpeed", animSpeed);
        UpdateAnimClipTimes();
    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = character.Animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Attack":
                    attackTime = clip.length / animSpeed;
                    break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        ICombatant combatant = other.GetComponent<ICombatant>();

        if (combatant != null && !combatantQueue.Contains(combatant))
        {

            combatant.OnCombatantKilled += Combatant_OnCombatantKilled;
            combatantQueue.Enqueue(combatant);
            if (CurrentTarget == null)
            {
                //hien tai neu khong co targetnao => lay thang dau tien luon
                CurrentTarget = combatantQueue.Peek();
                combatant.Detect();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ICombatant combatant = other.GetComponent<ICombatant>();

        if (combatant != null)
        {
            combatantQueue = new Queue<ICombatant>(combatantQueue.Where(e => e != combatant));
            //enemy.OnEnemyKilled -= Enemy_OnEnemyKilled; // Loại bỏ listener

            // Ẩn detected circle nếu có
            combatant.Undetect();

            // Nếu enemy hiện tại rời khỏi, cần chọn target mới hoặc ngừng tấn công
            if (CurrentTarget == combatant)
            {
                if (combatantQueue.Count > 0)
                {
                    // Có enemy khác trong hàng đợi, chọn làm mục tiêu mới
                    CurrentTarget = combatantQueue.Peek();
                    CurrentTarget.Detect();
                }
                else
                {
                    // Không còn enemy nào, cần ngừng tấn công
                    CurrentTarget = null;
                    IsAttacking = false;
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (CurrentTarget != null && !IsAttacking && !CurrentTarget.IsDead)
        {
            AttackCurrentEnemy();
        }
    }
    private void Combatant_OnCombatantKilled(ICombatant combatant)
    {
        OnDestroyEnemy(combatant);
        combatantQueue = new Queue<ICombatant>(combatantQueue.Where(e => !e.IsDead));
        UpdateTarget();
    }
    private void UpdateTarget()
    {
        if (combatantQueue.Count > 0)
        {
            CurrentTarget = combatantQueue.Peek();
            CurrentTarget.Detect();
        }
        else
        {
            CurrentTarget = null;
            IsAttacking = false;
        }
    }
    private void OnDestroyEnemy(ICombatant combatant)
    {
        combatant.OnCombatantKilled -= Combatant_OnCombatantKilled;
    }

    private void AttackCurrentEnemy()
    {
        if (!character.IsMoving)
        {
            StartCoroutine(WaitForAnimation());
        }
    }
    IEnumerator WaitForAnimation()
    {
        IsAttacking = true;
        character.LookAtTarget(currentTarget.GetTransform());
        character.ChangeAnim("attack");
        yield return new WaitForSeconds(attackTime / 2);
        character.HideWeapon();
        if (character.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            character.ThrowWeapon();
        }
        else
        {
            character.ShowWeapon();
        }
        yield return new WaitForSeconds(attackTime / 2);
        character.ShowWeapon();

        if (!character.IsMoving)
        {
            character.ChangeAnim("idle");
        }
        yield return new WaitForSeconds(0.1f);

        IsAttacking = false;
    }
}
