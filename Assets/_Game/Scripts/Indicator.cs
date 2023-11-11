using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Indicator : MonoBehaviour
{
    public GameObject target;
    public GameObject player;
    public Camera cam;
    public GameObject indicator;
    [SerializeField] private Canvas indicatorCanvas;
    public float offScreenThreshold = 10f;
    private bool IsVisible(Camera c, GameObject target)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        var point = target.transform.position;
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                return false;
            }
        }
        return true;
    }
    private void Update()
    {
        //vi la Screen Space  va la canvas nen can lay ReactTransform
        RectTransform indicatorRectTransform = indicator.GetComponent<RectTransform>();
        Vector3 targetScreenPos = cam.WorldToScreenPoint(target.transform.position);
        Vector2 targetCanvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            indicatorCanvas.transform as RectTransform, targetScreenPos, cam, out targetCanvasPosition);
        //vi day la Screen Space -camera nen can lay sizedelta
        Vector2 canvasSize = indicatorCanvas.GetComponent<RectTransform>().sizeDelta;
        indicatorRectTransform.localPosition = new Vector3(
            Mathf.Clamp(targetCanvasPosition.x, -canvasSize.x / 2 + offScreenThreshold, canvasSize.x / 2 - offScreenThreshold),
            Mathf.Clamp(targetCanvasPosition.y, -canvasSize.y / 2 + offScreenThreshold, canvasSize.y / 2 - offScreenThreshold)
        );

        Vector2 directionInCanvas = targetCanvasPosition - (Vector2)indicatorRectTransform.anchoredPosition;

        float angle = Mathf.Atan2(directionInCanvas.y, directionInCanvas.x) * Mathf.Rad2Deg - 90;


        indicatorRectTransform.localRotation = Quaternion.Euler(0, 0, angle);

    }
}
