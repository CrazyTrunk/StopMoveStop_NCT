using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private Transform infoCanvasTransform;
    [SerializeField] private Transform characterModel;
    [SerializeField] private float yOffset = 3.0f;
    [SerializeField] CanvasGroup canvasGroup;
    private void Awake()
    {
        SetCanvasVisibility(false);

    }
    private void OnEnable()
    {
        GameManager.Instance.OnStateChanged += HandleStateChange;
    }
    public void SetCanvasVisibility(bool isVisible)
    {
        canvasGroup.alpha = isVisible ? 1 : 0;
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;
    }
    private void HandleStateChange(GameState state)
    {
        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.IsState(GameState.Playing))
        {
            SetCanvasVisibility(true);
        }
        else
        {
            SetCanvasVisibility(false);
        }
    }

    private void OnDisable()
    {
        if (!GameManager.IsDestroying && GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleStateChange;
        }
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
