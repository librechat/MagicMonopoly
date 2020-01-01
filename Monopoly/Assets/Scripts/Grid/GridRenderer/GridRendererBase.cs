using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridRendererBase : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer panel;
    [SerializeField]
    private SpriteRenderer shade;
    
    void Awake()
    {
        
    }

    public virtual void Init(GridBase gridBehavior)
    {
        transform.position = gridBehavior.RenderPos;
    }

    public void LeavedGrid()
    {

    }

    public void PassedGrid()
    {
        MatchDisplayManager.InvokeAction(() =>
        {
            StartCoroutine(highlightCoroutine(0.3f));
        });
    }

    public void SteppedGrid()
    {
        MatchDisplayManager.InvokeAction(() =>
        {
            StartCoroutine(highlightCoroutine(0.7f));
        });
    }

    private IEnumerator highlightCoroutine(float strength)
    {
        Color originColor = shade.color;
        Color targetColor = new Color(1.0f, 1.0f, 1.0f, strength);
        Vector3 originScale = shade.transform.localScale;
        Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);

        yield return new WaitForSeconds(0.05f);

        IEnumerator transition = AnimTransition.Transition(EasingFunction.Ease.EaseOutCubic, 0.05f,
            (float ratio) =>
            {
                shade.color = Color.Lerp(originColor, targetColor, ratio);
            }
        );
        yield return StartCoroutine(transition);

        shade.color = targetColor;
        yield return null;

        transition = AnimTransition.Transition(EasingFunction.Ease.EaseInCubic, 0.15f,
            (float ratio) =>
            {
                shade.color = Color.Lerp(targetColor, originColor, ratio);
                shade.transform.localScale = Vector3.Lerp(originScale, targetScale, ratio);
            }
        );
        yield return StartCoroutine(transition);
        shade.color = originColor;
        shade.transform.localScale = originScale;

        yield return null;
    }
}
