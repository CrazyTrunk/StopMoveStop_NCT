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
    [SerializeField]private Canvas indicatorCanvas;
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
        RectTransform indicatorRectTransform = indicator.GetComponent<RectTransform>();
        Vector3 targetScreenPos = cam.WorldToScreenPoint(target.transform.position);
        targetScreenPos.z = 0;
        Vector2 targetCanvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            indicatorCanvas.transform as RectTransform, targetScreenPos, cam, out targetCanvasPosition);
        Vector2 directionInCanvas = targetCanvasPosition - (Vector2)indicatorRectTransform.anchoredPosition;
        float angle = Mathf.Atan2(directionInCanvas.y, directionInCanvas.x) * Mathf.Rad2Deg - 90;
        indicatorRectTransform.localRotation = Quaternion.Euler(0, 0, angle); 
    }
}
