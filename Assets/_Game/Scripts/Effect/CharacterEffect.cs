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
        if (currentEffectInstance != null)
        {
            Destroy(currentEffectInstance);
        }

        currentEffectInstance = Instantiate(deathEffect, parentHolder.position, Quaternion.identity, parentHolder);
    }
    public void DeactivateCurrentEffect()
    {
        if (currentEffectInstance != null)
        {
            Destroy(currentEffectInstance);
        }
    }
}
