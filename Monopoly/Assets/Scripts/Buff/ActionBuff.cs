using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBuff: Buff
{
    public ActionBuff(int level, BuffData data, Player owner): base(level,data,owner)
    {
        this.m_Level = level;
        this.m_Data = data;
        this.m_Owner = owner;
    }

    protected override void invoke()
    {
        base.invoke();

        int effectAmount = (m_Data.Effect == BuffData.BuffEffect.Level) ? m_Level : -1;
        ActionManager.InvokeAction(m_Data.BuffActionName, m_Owner, effectAmount);
    }
}