using UnityEngine;

public class KnifeBullet : Bullet
{
    public override void Shoot()
    {
        startPosition = transform.position;
        rb.velocity = transform.forward * range * attackSpeed;
    }
    public override void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }
}