using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [SerializeField]
    private SpellTable m_CircleSpellTable;
    // private SpellTable m_DirectSpellTable;
    [SerializeField]
    private SpellTable m_DestroySpellTable;
    // private SpellTable m_PassiveSpellTable;

    private static SpellManager s_Instance;

    private void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(this);
            return;
        }
        s_Instance = this;
    }

    public static List<SpellData> GetDefaultCircleSpells(){
        return s_Instance.m_CircleSpellTable.GetSpellDataAtLevel(0);
    }
    public static List<SpellData> GetDefaultDestroySpells()
    {
        return s_Instance.m_DestroySpellTable.GetSpellDataAtLevel(0);
    }

    public static SpellBase GetSpellObject(SpellData data)
    {
        if (data.Type == SpellData.SpellType.Circle)
        {
            return new CircleSpell(data);
        }
        else if (data.Type == SpellData.SpellType.Destroy)
        {
            return new DestroySpell(data);
        }
        else return null;
    }
}
