using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
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
            Vector3 targetDir = target.transform.position - player.transform.position;
            Debug.DrawRay(player.transform.position, targetDir, Color.green);
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
            indicator.transform.localEulerAngles = Vector3.forward * angle;
            Debug.Log(Vector3.forward * angle);

        }
        else
        {
            indicator.SetActive(false);
            Debug.Log("Target is visible.");
        }
    }
}
