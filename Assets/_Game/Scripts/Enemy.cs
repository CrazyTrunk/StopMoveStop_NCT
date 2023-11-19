using System;
using System.Collections;
using UnityEngine;

public class Enemy : Character, ICombatant
{
    [SerializeField] private DetectedCircle detectedCircle;
    private CapsuleCollider capsuleCollider;
    private Rigidbody rb;
    private bool isAnimPlay = false;
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
        ChangeAnim(Anim.DIE);
        if (!isAnimPlay)
        {
            StartCoroutine(WaitForAnimation());
        }
    }
    public void DeactiveEnemy()
    {
        capsuleCollider.isTrigger = true;
        rb.isKinematic = true;
    }
    IEnumerator WaitForAnimation()
    {
        isAnimPlay = true;
        float animationLength = Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSecondsRealtime(animationLength / 2);
        gameObject.SetActive(false);
        isAnimPlay = false;
    }
    public void Move(Vector3 direction)
    {
        transform.position += Speed * Time.deltaTime * direction;
    }
    public void StopMovement()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
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
