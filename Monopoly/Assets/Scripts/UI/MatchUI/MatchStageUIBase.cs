using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchStageUIBase : MonoBehaviour
{
    public virtual void Init(StageControllerBase controller)
    {
        controller.onEnterUIEvent += this.onEnter;
        controller.onExitUIEvent += this.onExit;
    }    
    protected virtual void onEnter()
    {

    }
    protected virtual void onExit()
    {

    }
    protected virtual void onUpdate()
    {

    }
}
