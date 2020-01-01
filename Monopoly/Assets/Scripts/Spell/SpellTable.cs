using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "CustomScriptableObject/SpellTable")]
public class SpellTable : GameDataTableBase<SpellData>
{
    public List<SpellData> SpellDataList { get { return DataList; } }

    // get spell list at specific level (default: all lvl 0)
    public List<SpellData> GetSpellDataAtLevel(int lvl = 0)
    {
        return DataList.Where(x => x.Level == lvl).ToList();
    }
}
