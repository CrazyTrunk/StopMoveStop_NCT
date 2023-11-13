
using UnityEngine;
[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Joystick joystick;
    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    private void FixedUpdate()
    {
        float yVelocity = rb.velocity.y;

        rb.velocity = new Vector3(joystick.Horizontal * moveSpeed, yVelocity, joystick.Vertical * moveSpeed);
        if (player.HasEnemyInSight && player.CanAttack)
        {
            player.ChangeAnim("attack");
            player.Throw(player.radicalTrigger.CurrentTargetEnemy);
        }
        else if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            // If we're moving, rotate and play the running animation, don't attack
            transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z));
            player.ChangeAnim("run");
        }
        else
        {
            // If we're not moving, go to idle
            player.ChangeAnim("idle");
        }
    }
}
