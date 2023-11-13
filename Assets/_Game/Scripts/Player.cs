using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private ThrowWeapon throwWeapon;
    [SerializeField] public RaidalTrigger radicalTrigger;

    public void TryToAttackEnemy(Enemy enemy)
    {
        LookAtTarget(enemy);
    }
    public void LookAtTarget(Enemy enemy)
    {
        transform.LookAt(enemy.transform.position);
    }
    
}
