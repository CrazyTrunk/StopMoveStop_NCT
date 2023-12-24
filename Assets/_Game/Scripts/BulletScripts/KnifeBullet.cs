using UnityEngine;

public class KnifeBullet : Bullet
{
    public override void Shoot()
    {
        startPosition = transform.position;
        Target = startPosition + (transform.forward * range);
        Vector3 directionToTarget = (Target - startPosition).normalized;

        transform.rotation = Quaternion.LookRotation(directionToTarget) * Quaternion.Euler(new Vector3(90f, 0, 180f));
    }
    public override void Update()
    {
        if (!isHitObstacle)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target, range * 2f * Time.deltaTime);
            if (Vector3.Distance(transform.position, Target) < 0.1f)
            {
                OnDespawn();
            }
        }
    }
}