using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class RadicalTrigger : MonoBehaviour
{
    private Queue<ICombatant> combatantQueue = new Queue<ICombatant>();
    private ICombatant currentTarget = null;
    private Character character;
    [SerializeField] private float animSpeed = 1.5f;
    [SerializeField] private float animPlayTime = 1f;

    public ICombatant CurrentTarget { get => currentTarget; set => currentTarget = value; }

    private void Awake()
    {
        character = GetComponentInParent<Character>();
        character.Animator.speed = animSpeed;
        //OnInit();
    }

    private void OnTriggerEnter(Collider other)
    {
        ICombatant combatant = other.GetComponent<ICombatant>();
        HandleAddEnemyToQueue(combatant);
    }
    private void HandleAddEnemyToQueue(ICombatant combatant)
    {
        if (combatant != null && !combatantQueue.Contains(combatant))
        {
            character.HasEnemyInSight = true;
            combatantQueue.Enqueue(combatant);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        ICombatant combatant = other.GetComponent<ICombatant>();

        HandleRemoveEnemyFromQueue(combatant);
    }
    private void HandleRemoveEnemyFromQueue(ICombatant combatant)
    {
        if (combatant != null)
        {
            //update Queue
            combatantQueue = new Queue<ICombatant>(combatantQueue.Where(e => e != combatant));
            if (CurrentTarget == combatant)
            {
                combatant.Undetect();
                CurrentTarget = combatantQueue.Peek();
                CurrentTarget.Detect();
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {

        UpdateTarget();
        if (CurrentTarget != null && !character.IsAttacking && !character.IsMoving)
        {
            HandleAttackCurrentEnemy();
        }
    }

    private void UpdateTarget()
    {
        if (combatantQueue.Count > 0)
        {
            if (CurrentTarget == null)
            {
                //Set Enemy from queue
                CurrentTarget = combatantQueue.Peek();
                CurrentTarget.Detect();
            }

        }
        else
        {
            CurrentTarget = null;
            character.IsAttacking = false;
            character.HasEnemyInSight = false;
        }
    }
    private void HandleAttackCurrentEnemy()
    {
        StartCoroutine(WaitForAnimation());
    }
    private void LookAtEnemyAndAttack()
    {
        character.LookAtTarget(currentTarget.GetTransform());
        character.ChangeAnim(Anim.ATTACK);
    }
    private void ThrowWeapon()
    {
        character.HideWeaponOnHand();
        CheckingAnimationAttackDone("Attack");
    }
    private void WhenDoneThrowWeapon()
    {
        character.ShowWeaponOnHand();
        if (!character.IsMoving)
        {
            character.ChangeAnim(Anim.IDLE);
        }
    }
    IEnumerator WaitForAnimation()
    {
        character.IsAttacking = true;
        LookAtEnemyAndAttack();
        yield return new WaitForSeconds((animPlayTime / animSpeed) / 2);
        ThrowWeapon();
        yield return new WaitForSeconds((animPlayTime / animSpeed) / 2);
        WhenDoneThrowWeapon();
        //wait for 0,4s before can attack again
        yield return new WaitForSeconds(0.4f);
        character.IsAttacking = false;
    }

    private void CheckingAnimationAttackDone(string animName)
    {
        if (character.Animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            character.ThrowWeapon();
        }
        else
        {
            character.ShowWeaponOnHand();
            character.IsAttacking = false;
            StopAllCoroutines();
        }
    }
    #region Event
    //private void Combatant_OnCombatantKilled(ICombatant combatant)
    //{
    //    if (combatant == null) return;

    //    OnDestroyEnemy(combatant);
    //    combatantQueue = new Queue<ICombatant>(combatantQueue.Where(e => !e.IsDead));
    //UpdateTarget();

    //    LevelManager.Instance.BotKilled(combatant);
    //}
    //public void OnInit()
    //{
    //    CurrentTarget = null;
    //    attackCoroutine = null;
    //}
    //private void OnDestroyEnemy(ICombatant combatant)
    //{
    //    combatant.OnCombatantKilled -= Combatant_OnCombatantKilled;
    //}
    //public void StopAttackCaroutine()
    //{
    //    if (attackCoroutine != null)
    //    {
    //        StopCoroutine(attackCoroutine);
    //        attackCoroutine = null;
    //    }
    //}
    #endregion
}
