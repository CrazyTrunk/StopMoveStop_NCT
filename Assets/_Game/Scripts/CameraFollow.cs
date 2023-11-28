using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    Vector3 offset;
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 playerView = new Vector3(0, 15f, -8f);

    private Vector3 _currentVelocity = Vector3.zero;
    private void Awake()
    {
        offset = transform.position - target.position;
    }
    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }
    public void SwitchCameraViewToPlayer()
    {
        offset = playerView;
        transform.rotation = Quaternion.Euler(70f, 0, 0);
    }
}
