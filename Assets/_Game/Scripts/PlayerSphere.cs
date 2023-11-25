using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSphere : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private ParticleSystem particalEffectOne;
    [SerializeField] private ParticleSystem particalEffectTwo;
    [SerializeField] private Character character;
    public void UpdateTriggerSize(float newRange)
    {
        sphereCollider.radius = newRange;
        if(character is Player)
        {
            var sh = particalEffectOne.shape;
            var sh2 = particalEffectTwo.shape;
            sh.radius = newRange;
            sh2.radius = newRange;
        }
     
    }
}
