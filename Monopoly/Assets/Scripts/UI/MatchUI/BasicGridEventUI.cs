using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BasicGridEventUI : MatchStageUIBase
{
    [SerializeField]
    private Canvas m_BuildCanvas;
    [SerializeField]
    private RectTransform m_CircleSpellPanel;
    [SerializeField]
    private RectTransform m_SpellButtonPrefab;
    private List<Button> m_SpellButtons;

    public override void Init(StageControllerBase controller)
    {
        BasicGridEventController stageCtrl = controller as BasicGridEventController;

        stageCtrl.onEnterBuildUIEvent += onBuildEnter;
        stageCtrl.onExitBuildUIEvent += onBuildExit;

        stageCtrl.onEnterUpgradeUIEvent += onUpgradeEnter;
        stageCtrl.onExitUpgradeUIEvent += onUpgradeExit;

        stageCtrl.onEnterTrappedUIEvent += onTrappedEnter;
        stageCtrl.onExitTrappedUIEvent += onTrappedExit;

        stageCtrl.onEnterDestroyUIEvent += onDestroyEnter;
        stageCtrl.onExitDestroyUIEvent += onDestroyExit;

        base.Init(controller);
    }

    protected override void onEnter()
    {
        base.onEnter();
    }
    protected override void onExit()
    {
        base.onEnter();
    }

    private void onBuildEnter(Player player, Action completedCallback)
    {
        List<SpellBase> spellList = player.SpellBook.CircleSpellList;
        enterSpellBtns(spellList, player, completedCallback);
    }
    private void onBuildExit()
    {
        exitSpellBtns();
    }
    private void onUpgradeEnter(Player player, Action completedCallback)
    {
        CircleBase circle = ((BasicGrid)GridManager.GetGrid(player.Position)).AttachedCircle;
        List<SpellBase> spellList = player.SpellBook.GetUpgradeSpells(circle);
        enterSpellBtns(spellList, player, completedCallback);
    }
    private void onUpgradeExit()
    {
        exitSpellBtns();
    }
    private void onDestroyEnter(Player player, Action completedCallback)
    {
        List<SpellBase> spellList = player.SpellBook.DestroySpellList;
        enterSpellBtns(spellList, player, completedCallback);
    }
    private void onDestroyExit()
    {
        exitSpellBtns();
    }
    private void onTrappedEnter()
    {

    }
    private void onTrappedExit()
    {

    }

    private void enterSpellBtns(List<SpellBase> spellList, Player player, Action completedCallback)
    {
        MatchDisplayManager.EnqueueAction((Action callback) =>
        {
            m_SpellButtons = new List<Button>();
            for (int i = 0; i <= spellList.Count; i++)
            {
                RectTransform btnTransform = Instantiate(m_SpellButtonPrefab);
                btnTransform.parent = m_CircleSpellPanel;
                btnTransform.anchoredPosition = Vector2.zero;

                Button btn = btnTransform.GetComponent<Button>();

                Text text = btn.GetComponentInChildren(typeof(Text)) as Text;
                // change icon: btn.GetComponent<Image>().sprite = ...

                if (i == spellList.Count)
                {
                    text.text = "Cancel";
                    btn.onClick.AddListener(() =>
                    {
                        completedCallback();
                    });
                }
                else
                {
                    text.text = spellList[i].Name;
                    SpellBase spell = spellList[i];
                    btn.onClick.AddListener(() =>
                    {
                        spell.Invoke(player);
                        completedCallback();
                    });
                    if (player.MP < spell.Cost) btn.interactable = false;
                }
                m_SpellButtons.Add(btn);
            }

            m_BuildCanvas.enabled = true;
            callback();
        });
    }
    private void exitSpellBtns()
    {
        MatchDisplayManager.EnqueueAction((Action callback) =>
        {
            m_BuildCanvas.enabled = false;

            for (int i = m_SpellButtons.Count - 1; i >= 0; i--)
            {
                m_SpellButtons[i].onClick.RemoveAllListeners();
                Destroy(m_SpellButtons[i].gameObject);
            }

            callback();
        });
    }

    #region UIEventCallback
    #endregion
}
