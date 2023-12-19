
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BoomerangScript : Bullet
{
    public enum State { Forward, Backward, Stop }

    private State state;
    public override void Shoot()
    {
        startPosition = transform.position;
        Target = startPosition + (transform.forward * range);
        Vector3 directionToTarget = (Target - startPosition).normalized;

        transform.rotation = Quaternion.LookRotation(directionToTarget) * Quaternion.Euler(new Vector3(90f, 0, 180f));
        state = State.Forward;
    }
    public override void Update()
    {
        switch (state)
        {
            case State.Forward:
                transform.position = Vector3.MoveTowards(transform.position, Target, range * 2f * Time.deltaTime);
                transform.Rotate(new Vector3(0f, 0f, 360f) * Time.deltaTime);
                if (Vector3.Distance(transform.position, Target) < 0.1f)
                {
                    state = State.Backward;
                }
                break;
            case State.Backward:
                transform.position = Vector3.MoveTowards(transform.position, startPosition, range * 2f * Time.deltaTime);
                transform.Rotate(new Vector3(0f, 0f, 360f) * Time.deltaTime);

                if (Vector3.Distance(startPosition, transform.position) < 0.1f)
                {
                    state = State.Stop;
                    OnDespawn();
                }
                break;
        }
       
    }
}
