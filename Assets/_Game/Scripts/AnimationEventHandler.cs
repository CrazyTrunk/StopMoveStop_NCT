using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    private event Action OnFinish;
    private void AnimationFinishedTrigger()
    {
        OnFinish?.Invoke();
    }
}
