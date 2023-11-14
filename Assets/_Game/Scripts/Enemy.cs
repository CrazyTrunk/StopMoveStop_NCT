using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : Character
{
    public int enemyNumber;
    private bool isAnimPlay = false;
    public delegate void EnemyKilledHandler(Enemy enemy);
    public event EnemyKilledHandler OnEnemyKilled;
    public void EnemyKilled()
    {
        IsDead = true;
        OnEnemyKilled?.Invoke(this);
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
}
