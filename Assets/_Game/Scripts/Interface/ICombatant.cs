using System;
using UnityEngine;

public interface ICombatant
{
    bool IsDead { get; set; }
    void Detect();
    void Undetect();
    Transform GetTransform();
    void LevelUp(int enemyLevel);

}

