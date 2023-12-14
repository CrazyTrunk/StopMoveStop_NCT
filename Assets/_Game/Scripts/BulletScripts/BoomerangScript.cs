
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BoomerangScript : Bullet
{
    public enum State { Forward, Backward, Stop }

    private State state;
    public override void Shoot()
    {
        startPosition = transform.position;
        rb.velocity = attackSpeed * range * transform.forward;
        transform.rotation = Quaternion.LookRotation(rb.velocity) * Quaternion.Euler(new Vector3(90f, 0, 180f));

        state = State.Forward;
    }
    public override void Update()
    {
        switch (state)
        {
            case State.Forward:
                if (Vector3.Distance(startPosition, transform.position) > range)
                {
                    state = State.Backward;
                    rb.velocity = -rb.velocity;
                }
                transform.Rotate(new Vector3(0f, 0f, 360f) * Time.deltaTime);
                break;
            case State.Backward:
                transform.Rotate(new Vector3(0f, 0f, 360f) * Time.deltaTime);

                if (Vector3.Distance(startPosition, transform.position) < 0.1f)
                {
                    rb.velocity = Vector3.zero;
                    state = State.Stop;
                    Destroy(gameObject);
                }
                break;
        }
       
    }
}
