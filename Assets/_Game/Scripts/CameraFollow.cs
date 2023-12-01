using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    Vector3 offset;
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 newCameraPos;
    [SerializeField] private Quaternion newRotationPos;
    private Vector3 originalPos;
    private Vector3 _currentVelocity = Vector3.zero;
    private bool isSwitching = false;
    private void Awake()
    {
        offset = transform.position - target.position;
        originalPos = offset;
    }
    private void Update()
    {
        if (!isSwitching)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
        }
    }
    public void SwitchCameraViewToPlayer()
    {
        StartCoroutine(SwitchToNewCameraView(newCameraPos, newRotationPos, 1f)); // 2f là thời gian chuyển đổi
    }
    IEnumerator SwitchToNewCameraView(Vector3 newPosition, Quaternion newRotation, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        isSwitching = true;

        while (time < duration)
        {
            // Sử dụng Lerp thay vì SmoothDamp để tránh vấn đề với _currentVelocity
            transform.position = Vector3.Lerp(startPosition, newPosition, time / duration);
            transform.rotation = Quaternion.Lerp(startRotation, newRotation, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        transform.position = newPosition; // Đặt rõ ràng để tránh bất kỳ sai số nào
        transform.rotation = newRotation;
        offset = newPosition;
        isSwitching = false;
    }
    public void UpdateCameraHeight()
    {
        offset = new Vector3(offset.x, offset.y + 2,offset.z);
        transform.Rotate(transform.rotation.x + 2, 0, 0, Space.Self);
    }
}
