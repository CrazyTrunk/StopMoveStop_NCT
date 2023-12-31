using TMPro;
using UnityEngine;

public class Indicator : MonoBehaviour
{

    [SerializeField] private RectTransform selfRect;
    [SerializeField] private float offScreenThreshold = 80f;
    [SerializeField] private TextMeshProUGUI levelDisplay;
    private Enemy target;
    //private Camera mainCamera;
    private RectTransform indicatorCanvas;
    public Enemy Target { get => target; set => target = value; }
    public void SetTarget(Enemy newTarget, RectTransform canvas, int level)
    {
        target = newTarget;
        target.OnLevelUp -= UpdateLevelUI;
        target.OnLevelUp += UpdateLevelUI;
        indicatorCanvas = canvas;
        levelDisplay.text = level.ToString();
    }

    private void UpdateLevelUI(int level)
    {
        levelDisplay.text = level.ToString();
    }

    public bool TargetIsOffScreen(Camera mainCamera)
    {
        Vector3 targetViewportPos = mainCamera.WorldToViewportPoint(target.transform.position);
        return targetViewportPos.x < 0 || targetViewportPos.x > 1 || targetViewportPos.y < 0 || targetViewportPos.y > 1;
    }
    public void UpdateIndicatorPosition(Camera mainCamera)
    {
        Vector3 adjustedPosition = calculateWorldPosition(target.transform.position, mainCamera);

        //khong gian man hinh
        Vector3 targetScreenPos = mainCamera.WorldToScreenPoint(adjustedPosition);

        Vector2 localPoint;
        //chuyen tu khon gian man hinh sang khong gian canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(indicatorCanvas, targetScreenPos, mainCamera, out localPoint);

        Vector2 canvasSize = indicatorCanvas.rect.size;
        //trung tam la (0,0) xy
        localPoint.x = Mathf.Clamp(localPoint.x, -canvasSize.x / 2 + offScreenThreshold, canvasSize.x / 2 - offScreenThreshold);
        localPoint.y = Mathf.Clamp(localPoint.y, -canvasSize.y / 2 + offScreenThreshold, canvasSize.y / 2 - offScreenThreshold);

        selfRect.localPosition = localPoint;
        RotateIndicatorTowardsTarget(localPoint);
    }
    private void RotateIndicatorTowardsTarget(Vector2 targetPosition)
    {
        float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg - 90;
        selfRect.localRotation = Quaternion.Euler(0, 0, angle);
    }
    // position = the world position of the entity to be tested
    private Vector3 calculateWorldPosition(Vector3 position, Camera camera)
    {
        //if the point is behind the camera then project it onto the camera plane
        Vector3 camNormal = camera.transform.forward;
        Vector3 vectorFromCam = position - camera.transform.position;
        float camNormDot = Vector3.Dot(camNormal, vectorFromCam.normalized);
        if (camNormDot <= 0f)
        {
            //we are beind the camera, project the position on the camera plane
            float camDot = Vector3.Dot(camNormal, vectorFromCam);
            Vector3 proj = (camNormal * camDot * 1.01f);   //small epsilon to keep the position infront of the camera
            position = camera.transform.position + (vectorFromCam - proj);
        }

        return position;
    }
}
