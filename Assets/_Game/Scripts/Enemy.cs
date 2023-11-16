using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : Character, ICombatant
{
    [SerializeField] private DetectedCircle detectedCircle;
    public int enemyNumber;
    private bool isAnimPlay = false;
    public event Action<ICombatant> OnCombatantKilled;
    public void EnemyKilled()
    {
        IsDead = true;
        OnCombatantKilled?.Invoke(this);
        if (!isAnimPlay)
        {
            StartCoroutine(WaitForAnimation());
        }
    }
    IEnumerator WaitForAnimation()
    {
        isAnimPlay = true;
        ChangeAnim("die");
        float animationLength = Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSecondsRealtime(animationLength / 2);
        gameObject.SetActive(false);
        isAnimPlay = false;
    }
    public void Move(Vector3 direction)
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    public void Detect()
    {
        detectedCircle.Show();
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
