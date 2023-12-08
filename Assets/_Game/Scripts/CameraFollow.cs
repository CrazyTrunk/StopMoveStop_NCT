using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    Vector3 offset;
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 mainCameraPos;
    [SerializeField] private Quaternion mainRotationPos;
    [SerializeField] private Vector3 shopCameraPos;
    [SerializeField] private Quaternion shopRotationPos;
    private Vector3 originalPos;
    private Quaternion originalRotation;

    private Vector3 _currentVelocity = Vector3.zero;
    private bool isSwitching = false;
    private void Awake()
    {
        originalPos = transform.position;
        originalRotation = transform.rotation;
    }
    public void OnInit(Transform target)
    {
        this.target = target;
        ResetCameraToOriginalPosition();
    }
    public void ResetCameraToOriginalPosition()
    {
        //transform.position = originalPos;
        //transform.rotation = originalRotation;
        transform.SetPositionAndRotation(originalPos, originalRotation);
        offset = transform.position - target.position;
        // Reset lại _currentVelocity để không ảnh hưởng đến việc di chuyển mượt mà của camera
        _currentVelocity = Vector3.zero;
        if (isSwitching)
        {
            StopAllCoroutines();
            isSwitching = false;
        }
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
        StartCoroutine(SwitchToNewCameraView(mainCameraPos, mainRotationPos, 1f)); // 2f là thời gian chuyển đổi
    }
    public void SwitchCameraViewToSkinShop()
    {
        StartCoroutine(SwitchToNewCameraView(shopCameraPos, shopRotationPos, 0.1f)); // 2f là thời gian chuyển đổi
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
            //transform.position = Vector3.Lerp(startPosition, newPosition, time / duration);
            //transform.rotation = Quaternion.Lerp(startRotation, newRotation, time / duration);
            transform.SetPositionAndRotation(Vector3.Lerp(startPosition, newPosition, time / duration), Quaternion.Lerp(startRotation, newRotation, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        //transform.position = newPosition; // Đặt rõ ràng để tránh bất kỳ sai số nào
        //transform.rotation = newRotation;
        transform.SetPositionAndRotation(newPosition, newRotation);
        offset = newPosition;
        isSwitching = false;
    }
    public void UpdateCameraHeight()
    {
        offset = new Vector3(offset.x, offset.y + 2,offset.z);
        transform.Rotate(transform.rotation.x + 2, 0, 0, Space.Self);
    }
}
