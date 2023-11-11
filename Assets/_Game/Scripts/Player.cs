using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private ThrowWeapon throwWeapon;
    private void Update()
    {
        if(CurrentAnim == "idle")
        {
            throwWeapon.Throw();
        }
    }
}
