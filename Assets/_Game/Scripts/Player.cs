using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character, ICombatant
{
    public event Action<ICombatant> OnCombatantKilled;

    public void Detect()
    {
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Undetect()
    {
    }
}
