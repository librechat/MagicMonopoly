using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class AnimTransition
{
    public static IEnumerator Transition(EasingFunction.Ease easing, float timeMax, Action<float> transition)
    {
        float timer = 0.0f;
        while (timer < timeMax)
        {
            float ratio = timer / timeMax;
            ratio = EasingFunction.GetEasingFunction(easing)(0.0f, 1.0f, ratio);

            // Lerp
            transition(ratio);

            timer += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}