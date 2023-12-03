
using System;
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
        if (GameManager.Instance.IsState(GameState.Playing))
        {
            rb.velocity = new Vector3(joystick.Horizontal * player.Speed, rb.velocity.y, joystick.Vertical * player.Speed);

            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                player.IsMoving = true;
                player.ChangeAnim(Anim.RUN);
            }
            else if ((joystick.Horizontal == 0 || joystick.Vertical == 0))
            {
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
