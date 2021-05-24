using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SpellData: GameDataBase
{ 
    public enum SpellType{
        Circle,
        Direct,
        Destroy,
        Passive
    };

    public string Name;
    public SpellType Type;

    public int Level;
    public int Cost;

    public int SpellEventId;

    #region FieldFunctions
    public override List<string> GetFieldNames()
    {
        List<string> s = base.GetFieldNames();
        string[] addition = new string[] {
            "Name", "Type", "Level", "Cost", "SpellEventId"
        };
        s.AddRange(addition);
        return s;
    }
    #endregion
}
