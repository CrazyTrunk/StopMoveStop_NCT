
using UnityEngine;
[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Joystick joystick;
    [SerializeField] private Transform infoCanvasTransform;
    private void Update()
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
            player.ChangeAnim(Anim.IDLE);
        }
    }
    private void LateUpdate()
    {
        infoCanvasTransform.LookAt(infoCanvasTransform.position +
            Camera.main.transform.rotation * Vector3.forward, Vector3.up);
    }
    private void FixedUpdate()
    {
        if (player.IsMoving)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
}
