using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private float smoothTime = 0.25f;
    private Transform target;
    [SerializeField] private Vector3 mainCameraPos;
    [SerializeField] private Quaternion mainRotationPos;
    [SerializeField] private Vector3 shopCameraPos;
    [SerializeField] private Quaternion shopRotationPos;
    private Vector3 originalPos;
    private Quaternion originalRotation;
    private Vector3 offset;

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
    private void LateUpdate()
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
    public void UpdateCamera(int characterLevel)
    {
        //offset = new Vector3(offset.x, offset.y + 5,offset.z);
        //transform.Rotate(transform.rotation.x + 5, 0, 0, Space.Self);
        UpdateCameraHeight(characterLevel);
        UpdateCameraRotation(characterLevel);
    }
    public void UpdateCameraHeight(int characterLevel)
    {
        float heightIncrease = Mathf.Floor(characterLevel) * 0.05f; // Tăng 5 đơn vị sau mỗi 10 cấp độ
        offset = new Vector3(offset.x, offset.y + heightIncrease, offset.z);
    }
    public void UpdateCameraRotation(int characterLevel)
    {
        float rotationIncrease = Mathf.Floor(characterLevel) * 0.05f; // Giả sử bạn muốn thay đổi góc 5 độ sau mỗi 10 cấp độ
        transform.Rotate(transform.rotation.x + rotationIncrease, 0, 0, Space.Self);
    }
}
