using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridData: GameDataBase
{
    public GridTypeData.GridTypeName TypeName;

    public Vector3 RenderPos;    
    public List<int> PreviousGridIdList;
    public List<int> NextGridIdList;

    #region FieldFunctions
    public override List<string> GetFieldNames()
    {
        List<string> s = base.GetFieldNames();
        string[] addition = new string[] {
            "TypeName", "RenderPos",
            "PreviousGridIdList", "NextGridIdList"
        };
        s.AddRange(addition);
        return s;
    }
    public override string GetField(System.Reflection.FieldInfo field)
    {
        return base.GetField(field);
    }
    public override void SetField(System.Reflection.FieldInfo field, string text)
    {
        base.SetField(field, text);
    }
    #endregion
}
