using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Indicator : MonoBehaviour
{
    [SerializeField] private float screenBoundOffset = 0.9f;

    public GameObject target;
    public GameObject player;
    public Camera cam;
    public GameObject indicator;

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

        if (!IsVisible(cam, target))
        {
            indicator.SetActive(true);
            Vector3 offScreenPos = cam.WorldToScreenPoint(target.transform.position);
            offScreenPos.x = Mathf.Clamp(offScreenPos.x, 0.05f, Screen.width / 2); // 0.05 and 0.95 to keep the indicator away from the very edge
            offScreenPos.y = Mathf.Clamp(offScreenPos.y, 0.05f, Screen.height / 2);
            Debug.Log(offScreenPos);
            //offScreenPos = cam.ViewportToWorldPoint(offScreenPos);
            indicator.transform.localPosition = new Vector3(offScreenPos.x, offScreenPos.y, 0);

            Vector3 playerPos = player.transform.position;
            Vector3 botPos = target.transform.position;
            Vector2 screenDirection = cam.WorldToScreenPoint(botPos) - cam.WorldToScreenPoint(playerPos);
            float angle = Mathf.Atan2(screenDirection.y, screenDirection.x) * Mathf.Rad2Deg;
            indicator.transform.localRotation = Quaternion.Euler(0, 0, angle - 90);
        }
        else
        {
            indicator.SetActive(false);
        }
    }
}
