using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public int Level { get { return m_Level; } set { m_Level = value; } }
    protected int m_Level;

    public BuffData Data { get { return m_Data; } }
    protected BuffData m_Data;

    protected Player m_Owner;

    public Buff(int level, BuffData data, Player owner)
    {
        this.m_Level = level;
        this.m_Data = data;
        this.m_Owner = owner;
    }


    public void Invoke()
    {
        if (m_Level == 0) return;

        invoke();
        reduceLife();

        return;
    }

    protected virtual void invoke(){

    }
    protected virtual void reduceLife()
    {
        if (m_Data.Life == BuffData.BuffLife.Level) m_Level--;
        else if (m_Data.Life == BuffData.BuffLife.OneRound) m_Level = 0;

        return;
    }
}
