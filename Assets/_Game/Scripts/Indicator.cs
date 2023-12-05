using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Indicator : MonoBehaviour
{
    private Transform target;
    private Camera mainCamera;
     private RectTransform indicatorCanvas;
    [SerializeField] private RectTransform selfRect;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float offScreenThreshold = 50f;

    public Transform Target { get => target; set => target = value; }

    private void Update()
    {
        if(Target != null)
        {
            if (TargetIsOffScreen())
            {
                UpdateIndicatorPosition();
                canvasGroup.alpha = 1;
            }
            else
            {
                canvasGroup.alpha = 0;
            }
        }
    }
    private void Start()
    {
        mainCamera = Camera.main;
    }
    public void SetTarget(Transform newTarget, RectTransform canvas)
    {
        Target = newTarget;
        indicatorCanvas = canvas;
    }
    private bool TargetIsOffScreen()
    {
        Vector3 targetViewportPos = mainCamera.WorldToViewportPoint(Target.position);
        return targetViewportPos.x < 0 || targetViewportPos.x > 1 || targetViewportPos.y < 0 || targetViewportPos.y > 1;
    }
    private void UpdateIndicatorPosition()
    {
        //khong gian man hinh
        Vector3 targetScreenPos = mainCamera.WorldToScreenPoint(Target.position);

        Vector2 localPoint;
        //chuyen tu khon gian man hinh sang khong gian canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(indicatorCanvas, targetScreenPos, mainCamera, out localPoint);

        Vector2 canvasSize = indicatorCanvas.sizeDelta;
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
}
