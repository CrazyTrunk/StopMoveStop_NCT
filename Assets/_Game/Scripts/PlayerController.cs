
using UnityEngine;
[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody rb;
    private Joystick joystick;
    public void InitJoyStick(Joystick joystick)
    {
        this.joystick = joystick;
    }
    private void Update()
    {
        if (player.IsDead || !GameManager.Instance.IsState(GameState.PLAYING))
        {
            rb.velocity = Vector3.zero;
            return;
        }
        if (GameManager.Instance.IsState(GameState.PLAYING))
        {

            Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

            if (direction.magnitude > 0.01f)
            {
                direction.Normalize();
                rb.velocity = direction * player.Speed;
                player.IsMoving = true;
                player.ChangeAnim(Anim.RUN);
            }
            else
            {
                rb.velocity = Vector3.zero;
                player.IsMoving = false;
                if (CanIdle())
                {
                    player.ChangeAnim(Anim.IDLE);
                }

            }
        }

    }

    private bool CanIdle()
    {
        return !player.HasEnemyInSight && !player.IsAttacking;
    }

    private void FixedUpdate()
    {
        if (player.IsMoving)
        {
            if (rb.velocity.magnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(rb.velocity);
            }
        }
    }
}
