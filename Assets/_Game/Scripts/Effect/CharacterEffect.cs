using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : MonoBehaviour
{
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private Transform parentHolder;

    private GameObject currentEffectInstance;

    public void ActiveDeathEffect()
    {
        currentEffectInstance = LeanPool.Spawn(deathEffect, parentHolder.position, Quaternion.identity, parentHolder);
    }
    public void DeactivateCurrentEffect()
    {
        if (currentEffectInstance != null)
        {
            LeanPool.Despawn(currentEffectInstance);
        }
    }
}
