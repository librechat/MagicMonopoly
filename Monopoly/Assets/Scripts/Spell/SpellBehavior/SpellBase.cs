using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBase
{
    public bool Available { get { return m_Available; } }
    protected bool m_Available;

    protected int m_SpellId;

    public string Name { get { return m_Name; } }
    protected string m_Name;

    protected int m_Level;
    public int Cost { get { return m_Cost; } set { m_Cost = value; } }
    protected int m_Cost;

    public SpellBase(SpellData data)
    {
        m_Available = true;
        m_SpellId = data.Id;
        m_Name = data.Name;
        m_Level = data.Level;
        m_Cost = data.Cost;
    }

    public SpellBase(SpellBase spell)
    {
        m_Available = true;
        m_SpellId = spell.m_SpellId;
        m_Name = spell.m_Name;
        m_Level = spell.m_Level;
        m_Cost = spell.Cost;
    }

    public virtual void Invoke(Player player)
    {
        // if(player.MP < m_Cost) return false;
        player.MP -= m_Cost;
    }
}
