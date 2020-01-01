using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerRenderer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_Mask;
    [SerializeField]
    private SpriteRenderer m_Face;
    [SerializeField]
    private SpriteRenderer m_Body;

    public void Init(Player player)
    {
        m_Body.color = player.Color;

        player.onHPChangedRenderEvent += onHPChanged;
        player.onMPChangedRenderEvent += onMPChanged;
        player.onCoinChangedRenderEvent += onCoinChanged;
        player.onBuffChangedRenderEvent += onBuffChanged;
    }

    public void onHPChanged(int prehp, int hp)
    {
        if (prehp == hp) return;

        MatchDisplayManager.EnqueueAction((callback) =>
        {
            IEnumerator anim = (prehp < hp) ? HPIncreaseCoroutine() : HPDecreaseCoroutine();
            StartCoroutine(anim);

            callback();
        });        
    }
    public void onMPChanged(int premp, int mp)
    {
    
    }
    public void onCoinChanged(int precoin, int coin)
    {

    }
    public void onBuffChanged()
    {

    }

    

    private IEnumerator HPDecreaseCoroutine()
    {
        float timeMax = 0.1f;
        
        Color originColor = m_Mask.color;
        Color targetColor = originColor;
        targetColor.a = 1.0f;

        IEnumerator transition = AnimTransition.Transition(EasingFunction.Ease.EaseOutCubic, timeMax,
            (float ratio) =>
            {
                m_Mask.color = Color.Lerp(originColor, targetColor, ratio);
            }
        );
        yield return StartCoroutine(transition);

        m_Mask.color = targetColor;
        yield return null;

        transition = AnimTransition.Transition(EasingFunction.Ease.EaseInCubic, timeMax,
            (float ratio) =>
            {
                m_Mask.color = Color.Lerp(targetColor, originColor, ratio);
            }
        );
        yield return StartCoroutine(transition);

        m_Mask.color = originColor;        
        yield return null;
    }
    private IEnumerator HPIncreaseCoroutine()
    {
        yield return null;
    }
}
