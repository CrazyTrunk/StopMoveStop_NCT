using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidalTrigger : MonoBehaviour
{
    public LineRenderer circleRenderer;
    public int steps = 100;
    public float radius = 1f;

    private void Start()
    {
        DrawCircle(steps, radius);
    }
    public void DrawCircle(int steps, float radius)
    {
        circleRenderer.positionCount = steps;
        for(int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, 0);
            circleRenderer.SetPosition(currentStep,currentPosition);
        }
    }
}
