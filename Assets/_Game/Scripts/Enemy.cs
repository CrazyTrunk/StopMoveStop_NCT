using System;
using System.Collections;
using UnityEngine;

public class Enemy : Character, ICombatant
{
    [SerializeField] private DetectedCircle detectedCircle;
    private RadicalTrigger radicalTrigger;

    private CapsuleCollider capsuleCollider;
    private Rigidbody rb;
    public event Action<ICombatant> OnCombatantKilled;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        radicalTrigger = FindObjectOfType<RadicalTrigger>();

    }
    public void EnemyKilled()
    {
        IsDead = true;
        StartCoroutine(WaitForAnimation(Anim.DIE));
        radicalTrigger.OnInit();
        OnCombatantKilled?.Invoke(this);
    }
    public void DeactiveEnemy()
    {
        capsuleCollider.isTrigger = true;
        rb.isKinematic = true;
    }

    private IEnumerator WaitForAnimation(string animName)
    {
        ChangeAnim(animName);
        yield return null;

        yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

    }
    public void Move(Vector3 direction)
    {
        transform.position += Speed * Time.deltaTime * direction;
    }
    public void Detect()
    {
        detectedCircle?.Show();
    }

    public void Undetect()
    {
        detectedCircle.Hide();
    }

    public Transform GetTransform()
    {
        return transform;
    }
    public void OnSpawn()
    {
        // Reset all the enemy states
        IsDead = false;
        detectedCircle.Hide();

        // Ensure the Rigidbody is active and configured for simulation
        rb.isKinematic = false;
        capsuleCollider.isTrigger = false;

        // Reset any necessary animations
        ChangeAnim("idle");

        // Clear event subscriptions to avoid duplicates
        OnCombatantKilled = null;
    }
}
