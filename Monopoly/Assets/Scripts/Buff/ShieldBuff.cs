using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ShieldBuff : Buff
{
    private int m_CounterAmount = 0;
    
    public ShieldBuff(int level, BuffData data, Player owner)
        : base(level, data, owner)
    {
        this.m_Level = level;
        this.m_Data = data;
        this.m_Owner = owner;
    }

    public int Invoke(int counter)
    {
        m_CounterAmount = counter;
        base.Invoke();
        return m_CounterAmount;
    }

    protected override void invoke()
    {
        base.invoke();

        if (m_CounterAmount == 0) return;
        m_CounterAmount = (m_Level > m_CounterAmount) ? 0 : m_CounterAmount - m_Level;
    }
    protected override void reduceLife()
    {
        base.reduceLife();
        if (m_Data.Life == BuffData.BuffLife.Shield)
        {
            m_Level = (m_Level > m_CounterAmount) ? m_Level - m_CounterAmount : 0;
        }

        return;
    }
}
