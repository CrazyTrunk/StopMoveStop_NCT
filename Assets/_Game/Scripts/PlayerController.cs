
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

        rb.velocity = new Vector3 (joystick.Horizontal * moveSpeed, yVelocity, joystick.Vertical * moveSpeed);
        if ((joystick.Horizontal != 0 || joystick.Vertical != 0) )
        {
            player.CanAttack = false;
            transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z));
            player.ChangeAnim("run");
        }
        else 
        {
           player.ChangeAnim("idle");
           player.CanAttack = true;

        }
    }
}
