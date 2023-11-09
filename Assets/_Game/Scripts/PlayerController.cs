using System.Collections;
using System.Collections.Generic;
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
        rb.velocity = new Vector3 (joystick.Horizontal * moveSpeed, rb.velocity.y, joystick.Vertical * moveSpeed);
        if((joystick.Horizontal != 0 || joystick.Vertical != 0) && rb.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            player.ChangeAnim("run");
        }
        else if(joystick.Horizontal == 0 || joystick.Vertical == 0)
        {
            player.ChangeAnim("idle");
        }
    }
}
