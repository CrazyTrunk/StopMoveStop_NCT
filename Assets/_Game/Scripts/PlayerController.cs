
using UnityEngine;
[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Joystick joystick;

    // Start is called before the first frame update
    private void Update()
    {
        rb.velocity = new Vector3(joystick.Horizontal * player.Speed, rb.velocity.y, joystick.Vertical * player.Speed);
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            player.IsMoving = true;
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            player.ChangeAnim("run");
        }
        else if ((joystick.Horizontal == 0 || joystick.Vertical == 0))
        {
            player.IsMoving = false;
            player.ChangeAnim("idle");

        }
    }
}
