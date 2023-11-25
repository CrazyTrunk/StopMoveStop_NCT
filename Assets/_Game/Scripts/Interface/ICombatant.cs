using System;
using UnityEngine;

public interface ICombatant
{
    bool IsDead { get; set; }
    ICombatant LastAttacker { get; set; }
    event Action<ICombatant> OnCombatantKilled;
    void Detect();
    void Undetect();
    Transform GetTransform();
    void LevelUp(int enemyLevel);

}

