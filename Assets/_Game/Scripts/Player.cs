using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character, ICombatant
{
    public event Action<ICombatant> OnCombatantKilled;

    public void Detect()
    {
        throw new NotImplementedException();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Undetect()
    {
        throw new NotImplementedException();
    }
}
