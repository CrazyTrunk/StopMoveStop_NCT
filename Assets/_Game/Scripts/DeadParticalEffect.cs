using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadParticalEffect : MonoBehaviour
{
    [SerializeField]private ParticleSystem deadParticle;
    private bool hasPlay = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.BULLET) && !hasPlay)
        {
            PlayDeadEffect();
        }
    }
    private void PlayDeadEffect()
    {
        var em = deadParticle.emission;
        em.enabled = true;
        deadParticle.Play();
        hasPlay = true;
        StartCoroutine(WaitForParticleToFinish());
    }
    private IEnumerator WaitForParticleToFinish()
    {
        yield return new WaitForSeconds(deadParticle.main.duration);
        var em = deadParticle.emission;
        deadParticle.Clear();
        deadParticle.Stop();
        em.enabled = false;
        hasPlay = false;
    }
}
