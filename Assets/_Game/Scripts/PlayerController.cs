using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Animator amimator;
    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        rb.velocity = new Vector3 (joystick.Horizontal * moveSpeed, rb.velocity.y, joystick.Vertical * moveSpeed);
    }
}
