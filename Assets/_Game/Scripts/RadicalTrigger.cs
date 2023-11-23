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


    public ICombatant CurrentTarget { get => currentTarget; set => currentTarget = value; }

    private void Awake()
    {
        character = GetComponentInParent<Character>();
        OnInit();
    }

    private void OnTriggerEnter(Collider other)
    {
        ICombatant combatant = other.GetComponent<ICombatant>();
        HandleAddEnemyToQueue(combatant);
    }
    public void OnInit()
    {
        CurrentTarget = null;
        StopAllCoroutines();
        combatantQueue = new Queue<ICombatant>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (character.IsDead || (CurrentTarget != null && CurrentTarget.IsDead))
        {
            return; 
        }
        UpdateTarget();
        if (CurrentTarget != null && !character.IsAttacking && !character.IsMoving)
        {
            HandleAttackCurrentEnemy();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        ICombatant combatant = other.GetComponent<ICombatant>();

        HandleRemoveEnemyFromQueue(combatant);
    }
    private void HandleAddEnemyToQueue(ICombatant combatant)
    {
        if (combatant != null && !combatantQueue.Contains(combatant))
        {
            character.HasEnemyInSight = true;
            combatantQueue.Enqueue(combatant);
            combatant.OnCombatantKilled += Combatant_OnCombatantKilled;
        }
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
                combatant.OnCombatantKilled -= Combatant_OnCombatantKilled;
                //if theres still enemy in Queue
                if (combatantQueue.Count > 0)
                {
                    GetTheFirstEnemyFromQueue();
                }
            }

        }
    }
    private void GetTheFirstEnemyFromQueue()
    {
        CurrentTarget = combatantQueue.Peek();
        CurrentTarget.Detect();
    }

    private void UpdateTarget()
    {
        if (combatantQueue.Count > 0)
        {
            if (CurrentTarget == null)
            {
                //Set Enemy from queue
                GetTheFirstEnemyFromQueue();
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
    }
    IEnumerator WaitForAnimation()
    {
        character.IsAttacking = true;
        LookAtEnemyAndAttack();
        yield return new WaitForSeconds((character.AnimPlayTime / character.AnimSpeed) / 2);
        ThrowWeapon();
        yield return new WaitForSeconds((character.AnimPlayTime / character.AnimSpeed) / 2);
        WhenDoneThrowWeapon();
        //wait for 0,4s before can attack again
        yield return new WaitForSeconds(0.6f);
        character.IsAttacking = false;
    }

    private void CheckingAnimationAttackDone(string animName)
    {
        //If Anim is Finish
        if (character.Animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            character.ThrowWeapon();
        }
        else
        {
            //Anim is cancled
            character.ShowWeaponOnHand();
            character.IsAttacking = false;
            StopAllCoroutines();
        }
    }
    #region Event
    private void Combatant_OnCombatantKilled(ICombatant combatant)
    {
        combatant.OnCombatantKilled -= Combatant_OnCombatantKilled;
        //Update Queue
        if (CurrentTarget == combatant)
        {
            CurrentTarget = null;
            combatantQueue = new Queue<ICombatant>(combatantQueue.Where(e => e != combatant));
            LevelManager.Instance.BotKilled(combatant);
        }
    }
    #endregion
}
