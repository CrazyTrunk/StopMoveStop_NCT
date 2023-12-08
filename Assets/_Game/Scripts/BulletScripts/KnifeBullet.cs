using UnityEngine;

public class KnifeBullet : Bullet
{
    public override void Shoot()
    {
        startPosition = transform.position;
        rb.velocity = attackSpeed * range * transform.forward;
        transform.rotation = Quaternion.LookRotation(rb.velocity) * Quaternion.Euler(new Vector3(90f,0, 180f));
    }
    public override void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }
}