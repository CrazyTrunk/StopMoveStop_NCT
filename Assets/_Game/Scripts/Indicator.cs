using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Indicator : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject indicator;
    [SerializeField] private Canvas indicatorCanvas;
    [SerializeField] private float offScreenThreshold = 50f;
    private void Update()
    {
        if (TargetIsOffScreen())
        {
            UpdateIndicatorPosition();
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }
    }
    private bool TargetIsOffScreen()
    {
        Vector3 targetViewportPos = mainCamera.WorldToViewportPoint(target.transform.position);
        return targetViewportPos.x < 0 || targetViewportPos.x > 1 || targetViewportPos.y < 0 || targetViewportPos.y > 1;
    }
    private void UpdateIndicatorPosition()
    {
        RectTransform indicatorRect = indicator.GetComponent<RectTransform>();
        Vector3 targetScreenPos = mainCamera.WorldToScreenPoint(target.transform.position);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)indicatorCanvas.transform, targetScreenPos, mainCamera, out localPoint);

        Vector2 canvasSize = ((RectTransform)indicatorCanvas.transform).sizeDelta;
        localPoint.x = Mathf.Clamp(localPoint.x, -canvasSize.x / 2 + offScreenThreshold, canvasSize.x / 2 - offScreenThreshold);
        localPoint.y = Mathf.Clamp(localPoint.y, -canvasSize.y / 2 + offScreenThreshold, canvasSize.y / 2 - offScreenThreshold);

        indicatorRect.localPosition = localPoint;
        RotateIndicatorTowardsTarget(localPoint);
    }
    private void RotateIndicatorTowardsTarget(Vector2 targetPosition)
    {
        RectTransform indicatorRect = indicator.GetComponent<RectTransform>();
        float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg - 90;
        indicatorRect.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
