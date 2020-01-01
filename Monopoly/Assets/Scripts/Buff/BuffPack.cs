using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BuffPack
{
    private List<Buff> m_Buffs;
    private Player m_Owner;

    public void Init(Player owner)
    {
        m_Owner = owner;
        m_Buffs = new List<Buff>();
        List<BuffData> buffList = ActionManager.BuffTable.BuffDataList;
        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i].Type == BuffData.BuffType.Action) m_Buffs.Add(new ActionBuff(0, buffList[i], owner));
            else if (buffList[i].Type == BuffData.BuffType.Shield) m_Buffs.Add(new ShieldBuff(0, buffList[i], owner));
            // else if (buffList[i].Type == BuffData.BuffType.Restrict)
            else m_Buffs.Add(new Buff(0, buffList[i], owner));
        }
    }

    public void Add(string buffName, int level)
    {
        for (int i = 0; i < m_Buffs.Count; i++)
        {
            if (m_Buffs[i].Data.Name == buffName)
            {
                m_Buffs[i].Level += level;
                m_Owner.onBuffChangedUIEvent(m_Buffs[i].Level);
                m_Owner.onBuffChangedRenderEvent();
            }
        }
    }

    public void Update(BuffData.BuffTiming timing, BuffData.BuffType type = BuffData.BuffType.Action)
    {
        for (int i = 0; i < m_Buffs.Count; i++)
        {
            if (m_Buffs[i].Data.Timing == timing && m_Buffs[i].Data.Type == type)
            {
                m_Buffs[i].Invoke();
            }
        }
        
        return;
    }
    public int UpdateShield(BuffData.BuffTiming timing, ActionData.ActionType counterTarget, int amt)
    {
        int amount = amt;
        string counterName = counterTarget.ToString();
        for (int i = 0; i < m_Buffs.Count; i++)
        {
            if (m_Buffs[i].Data.Timing == timing
                && m_Buffs[i].Data.Type == BuffData.BuffType.Shield
                && m_Buffs[i].Data.BuffActionName == counterName)
            {
                ShieldBuff shield = m_Buffs[i] as ShieldBuff;
                amount = shield.Invoke(amount);
            }
        }
        return amount;
    }
}