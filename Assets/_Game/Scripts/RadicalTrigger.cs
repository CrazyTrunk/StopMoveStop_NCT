﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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
        combatantQueue.Clear();
        StopAllCoroutines();
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.IsState(GameState.PLAYING))
        {
            if (!character.IsAttacking)
            {
                UpdateTarget();
                if (CurrentTarget != null && CanAttack())
                {
                    WeaponType weaponType = character.WeaponData.type;
                    StartCoroutine(AttackCoroutine(weaponType));
                }
            }
        }

    }

    private bool CanAttack()
    {
        return character != null && !character.IsDead && CurrentTarget != null && !CurrentTarget.IsDead && !character.IsMoving;
    }

    private void OnTriggerExit(Collider other)
    {
        ICombatant combatant = other.GetComponent<ICombatant>();

        HandleRemoveEnemyFromQueue(combatant);
    }
    private void HandleAddEnemyToQueue(ICombatant combatant)
    {
        if (combatant != null && !combatantQueue.Contains(combatant) && !combatant.IsDead)
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
                CurrentTarget = null;
                if (character is Player)
                {
                    combatant.Undetect();
                }
                combatant.OnCombatantKilled -= Combatant_OnCombatantKilled;
                //if theres still enemy in Queue
                if (combatantQueue.Count > 0)
                {
                    GetTheFirstEnemyFromQueue();
                }
                else
                {
                    character.HasEnemyInSight = false;
                }
            }

        }
    }
    private void GetTheFirstEnemyFromQueue()
    {
        CurrentTarget = combatantQueue.Peek();
        if (character is Player)
        {
            CurrentTarget.Detect();
        }
    }

    private void UpdateTarget()
    {
        if (combatantQueue.Count > 0 && CurrentTarget == null)
        {
            //Set Enemy from queue
            GetTheFirstEnemyFromQueue();
        }
        else if (combatantQueue.Count == 0)
        {
            CurrentTarget = null;
            character.HasEnemyInSight = false;
            StopAllCoroutines();
        }
    }
    private void LookAtEnemyAndAttack()
    {
        character.LookAtTarget(currentTarget.GetTransform());
    }
    private void ThrowWeapon(string animName)
    {
        character.HideWeaponOnHand();
        CheckingAnimationAttackDone(animName);
    }
    private void WhenDoneThrowWeapon()
    {
        character.ShowWeaponOnHand();
    }
    IEnumerator AttackCoroutine(WeaponType weaponType)
    {
        character.IsAttacking = true;
        LookAtEnemyAndAttack();
        var (attackAnim, animName) = character.GetAttackAnimation(weaponType); 
        character.ChangeAnim(attackAnim);
        yield return new WaitForSeconds((character.AnimPlayTime / character.AnimSpeed) / 2);
        ThrowWeapon(animName);
        yield return new WaitForSeconds((character.AnimPlayTime / character.AnimSpeed) / 2);
        WhenDoneThrowWeapon();
        //wait for 0,4s before can attack again
        yield return new WaitForSeconds(0.2f);
        if (!character.IsDead && !character.IsMoving)
        {
            character.ChangeAnim(Anim.IDLE);
        }
        character.IsAttacking = false;
    }

    private void CheckingAnimationAttackDone(string animName)
    {
        //If Anim is Finish
        if (character.Animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            AudioManager.Instance.PlaySFX(SoundType.THROW);
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
        if (combatantQueue.Contains(combatant))
        {
            combatant.OnCombatantKilled -= Combatant_OnCombatantKilled;
            combatantQueue = new Queue<ICombatant>(combatantQueue.Where(e => e != combatant));
            if (CurrentTarget == combatant)
            {
                CurrentTarget = null;
                character.HasEnemyInSight = false;
            }
        }
    }
    #endregion

}
