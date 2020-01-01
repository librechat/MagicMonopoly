using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveStageUI : MatchStageUIBase
{
    [SerializeField]
    private Button m_DrawDiceBtn;

    public override void Init(StageControllerBase controller)
    {
        MoveStageController moveStageCtrl = controller as MoveStageController;

        moveStageCtrl.onEnterDrawUIEvent += onDrawEnter;
        moveStageCtrl.onExitDrawUIEvent += onDrawExit;

        moveStageCtrl.onEnterBranchUIEvent += onBranchEnter;
        moveStageCtrl.onExitBranchUIEvent += onBranchExit;
                
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

    private void onDrawEnter()
    {
        MatchDisplayManager.EnqueueAction((Action callback) =>
        {
            toggleDiceBtn(true);
            callback();
        });       
    }
    private void onDrawExit()
    {
        MatchDisplayManager.InvokeAction(() =>
        {
            toggleDiceBtn(false);
        });
    }
    private void onBranchEnter()
    {
    }
    private void onBranchExit()
    {
    }
    private void toggleDiceBtn(bool toggle)
    {
        m_DrawDiceBtn.interactable = toggle;
    }

    #region UIEventCallback
    public void DrawDice()
    {
        MatchManager.OnDiceDrawed();
    }
    #endregion
}
