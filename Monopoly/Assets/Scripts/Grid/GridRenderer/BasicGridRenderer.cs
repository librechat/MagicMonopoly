using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGridRenderer : GridRendererBase
{
    [SerializeField]
    private SpriteMask ringMask;
    [SerializeField]
    private List<SpriteRenderer> rings;

    private int level = -1;
    private CircleData.CircleAttribute attribute;

    public override void Init(GridBase gridBehavior)
    {
        base.Init(gridBehavior);

        BasicGrid basicBehavior = gridBehavior as BasicGrid;
        basicBehavior.onCircleChangedRenderEvent += onCircleChanged;
    }

    private void onCircleChanged(int level, CircleData.CircleAttribute attr, Color ownerColor)
    {
        MatchDisplayManager.InvokeAction(() =>
        {
            StartCoroutine(changeCircleCoroutine(level, attr, ownerColor));
        });
    }

    private IEnumerator changeCircleCoroutine(int level, CircleData.CircleAttribute attr, Color color)
    {
        if (this.level < level)
        {
            yield return StartCoroutine(attachRingCoroutine(this.level + 1, level, color));
        }
        else if (this.level > level)
        {
            yield return StartCoroutine(deattachRingCoroutine(this.level, level + 1));
        }

        this.level = level;
        this.attribute = attr;
        yield return null;
    }

    private IEnumerator attachRingCoroutine(int startLvl, int endLvl, Color color)
    {
        Color originColor = color;
        originColor.a = 0.0f;
        Color targetColor = color;
        targetColor.a = 1.0f;

        Vector3 originScale = new Vector3(1.2f, 1.2f, 1.2f);
        Vector3 targetScale = new Vector3(1.0f, 1.0f, 1.0f);

        for (int i = startLvl; i <= endLvl; i++)
        {
            rings[i].color = originColor;
            rings[i].transform.localScale = originScale;
        }
        ringMask.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        IEnumerator transition = AnimTransition.Transition(EasingFunction.Ease.EaseOutCubic, 0.3f,
            (float ratio) =>
            {
                for (int i = startLvl; i <= endLvl; i++)
                {
                    rings[i].color = Color.Lerp(originColor, targetColor, ratio);
                    rings[i].transform.localScale = Vector3.Lerp(originScale, targetScale, ratio);
                }
            }
        );
        yield return StartCoroutine(transition);

        for (int i = startLvl; i <= endLvl; i++)
        {
            rings[i].color = targetColor;
            rings[i].transform.localScale = targetScale;
        }
        yield return null;
    }
    private IEnumerator deattachRingCoroutine(int startLvl, int endLvl)
    {
        // index = 0: 0.8~1, index = 1: 0.6~0.8, index = 2: 0.4~0.6
        float originW = 1.0f - 0.2f * (startLvl + 1);
        float targetW = originW + 0.2f * (startLvl - endLvl + 1);
        Vector3 originScale = new Vector3(originW, originW, originW);
        Vector3 targetScale = new Vector3(targetW, targetW, targetW);

        ringMask.transform.localScale = originScale;

        IEnumerator transition = AnimTransition.Transition(EasingFunction.Ease.EaseOutSine, 0.2f,
            (float ratio) =>
            {
                ringMask.transform.localScale = Vector3.Lerp(originScale, targetScale, ratio);
            }
        );
        yield return StartCoroutine(transition);

        ringMask.transform.localScale = targetScale;

        for (int i = endLvl; i <= startLvl; i++)
        {
            Color color = rings[i].color;
            color.a = 0.0f;
            rings[i].color = color;
        }
        yield return null;
    }
}
