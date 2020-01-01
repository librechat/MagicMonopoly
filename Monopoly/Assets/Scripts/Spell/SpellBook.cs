using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpellBook
{
    private int m_Capacity = 5;

    public List<SpellBase> CircleSpellList { get { return m_CircleSpellList; } }
    private List<SpellBase> m_CircleSpellList;

    public List<SpellBase> DestroySpellList { get { return m_DestroySpellList; } }
    private List<SpellBase> m_DestroySpellList;

    public void Init()
    {        
        m_CircleSpellList = new List<SpellBase>();
        m_DestroySpellList = new List<SpellBase>();

        List<SpellData> circleSpells = SpellManager.GetDefaultCircleSpells();
        for (int i = 0; i < circleSpells.Count; i++ ){
            AddSpell(circleSpells[i]);
        }
        List<SpellData> destroySpells = SpellManager.GetDefaultDestroySpells();
        for (int i = 0; i < destroySpells.Count; i++)
        {
            AddSpell(destroySpells[i]);
        }
    }

    public bool AddSpell(SpellData data)
    {
        if (data.Type == SpellData.SpellType.Circle)
        {
            if (m_CircleSpellList.Count == m_Capacity) return false;
            else
            {
                SpellBase spell = SpellManager.GetSpellObject(data);
                if (spell != null)
                {
                    m_CircleSpellList.Add(spell);
                }

                return true;
            }
        }
        else if (data.Type == SpellData.SpellType.Destroy)
        {
            if (m_DestroySpellList.Count == m_Capacity) return false;
            else
            {
                SpellBase spell = SpellManager.GetSpellObject(data);
                if (spell != null)
                {
                    m_DestroySpellList.Add(spell);
                }

                return true;
            }
        }
        else return false;
    }

    public List<SpellBase> GetUpgradeSpells(CircleBase circle)
    {
        List<int> succCircleList = circle.SucceedingIdList;
        SpellBase circleSpell = m_CircleSpellList.Find((s) => { return ((CircleSpell)s).CircleId == circle.CircleId; });
        return m_CircleSpellList.Where(
            (s) =>
            {
                return succCircleList.FindIndex((c) =>
                {
                    return c == ((CircleSpell)s).CircleId;
                }) != -1;
            }
        ).Select(
            (s) => 
            {
                SpellBase spell = new CircleSpell((CircleSpell)s);
                spell.Cost -= circleSpell.Cost;
                return spell; 
            }
        ).ToList();

    }
}
