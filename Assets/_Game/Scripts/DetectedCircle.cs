using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectedCircle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void Show()
    {
        _spriteRenderer.enabled = true;
    }
    public void Hide()
    {
        _spriteRenderer.enabled = false;
    }
}
