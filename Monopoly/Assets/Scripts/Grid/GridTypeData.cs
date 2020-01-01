using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GridTypeData : GameDataBase
{
    public enum GridTypeName
    {
        Basic,
        Home,
        Shop,
        Event
    }

    public GridTypeName Name;
    public Transform Prefab;

    #region FieldFunctions
    public override List<string> GetFieldNames()
    {
        List<string> s = base.GetFieldNames();
        string[] addition = new string[] {
            "Name"
        };
        s.AddRange(addition);
        return s;
    }
    public override string GetField(System.Reflection.FieldInfo field)
    {
        if (field.FieldType == typeof(GridTypeName)) return field.GetValue(this).ToString();
        else return base.GetField(field);
    }
    public override void SetField(System.Reflection.FieldInfo field, string text)
    {
        if (field.FieldType == typeof(GridTypeName))
        {
            Name = (GridTypeName)Enum.Parse(typeof(GridTypeName), text);
        }
        else base.SetField(field, text);
    }
    #endregion
}