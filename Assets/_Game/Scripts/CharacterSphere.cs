using UnityEngine;

public class CharacterSphere : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private ParticleSystem particalEffectOne;
    [SerializeField] private ParticleSystem particalEffectTwo;
    [SerializeField] private Character character;
    public void UpdateTriggerSize(float newRange)
    {
        sphereCollider.radius = newRange;
        if (character is Player)
        {
            UpdateCharacterParticalRange(newRange);
        }
    }
    private void UpdateCharacterParticalRange(float newRange)
    {
        var sh = particalEffectOne.shape;
        var sh2 = particalEffectTwo.shape;
        sh.radius = newRange;
        sh2.radius = newRange;
        particalEffectOne.Play();
        particalEffectTwo.Play();

    }
}
