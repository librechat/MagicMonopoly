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
            "Name", "Type", "Level", "Cost", "SpellEventName",
            "PrecedingsIdList", "SucceedingsIdList"
        };
        s.AddRange(addition);
        return s;
    }
    public override string GetField(System.Reflection.FieldInfo field)
    {
        if (field.FieldType == typeof(SpellType)) return field.GetValue(this).ToString();
        else return base.GetField(field);
    }
    public override void SetField(System.Reflection.FieldInfo field, string text)
    {
        if (field.FieldType == typeof(SpellType))
        {
            Type = (SpellType)Enum.Parse(typeof(SpellType), text);
        }
        else base.SetField(field, text);
    }
    #endregion
}
