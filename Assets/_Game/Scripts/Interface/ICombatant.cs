using System;
using UnityEngine;

public interface ICombatant
{
    event Action<ICombatant> OnCombatantKilled;
    bool IsDead { get; set; }
    void Detect();
    void Undetect();
    Transform GetTransform();
    void LevelUp(int enemyLevel);

}

