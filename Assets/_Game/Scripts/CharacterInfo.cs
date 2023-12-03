using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private Transform infoCanvasTransform;
    [SerializeField] private Transform characterModel;
    [SerializeField] private float yOffset = 3.0f;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        GlobalEvents.OnPlayClick += SetActiveTrue;
    }

    private void OnDestroy()
    {
        GlobalEvents.OnPlayClick -= SetActiveTrue;
    }
    private void SetActiveTrue()
    {
        gameObject.SetActive(true);
    }

    private void LateUpdate()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        float scaledYOffset = yOffset + characterModel.transform.localScale.y;

        Vector3 newPos = characterModel.transform.position + Vector3.up * scaledYOffset;
        infoCanvasTransform.position = newPos;
        infoCanvasTransform.LookAt(newPos + cameraForward, Vector3.up);
    }
}
