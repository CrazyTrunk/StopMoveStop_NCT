using System;
using System.Collections;
using UnityEngine;

public class Enemy : Character, ICombatant
{
    [SerializeField] private DetectedCircle detectedCircle;
    private CapsuleCollider capsuleCollider;
    private Rigidbody rb;
    public event Action<ICombatant> OnCombatantKilled;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
    }
    public void EnemyKilled()
    {
        IsDead = true;
        OnCombatantKilled?.Invoke(this);
        StartCoroutine(WaitForAnimation(Anim.DIE));
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
        gameObject.SetActive(false);

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
}
