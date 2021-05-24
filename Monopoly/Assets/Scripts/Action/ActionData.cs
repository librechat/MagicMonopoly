using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionData: GameDataBase
{
    public enum ActionType{
        Empty,
        HP,
        MP,
        Coin,
        Buff, // include step buff and other buffs
        Step
    }
    public enum TargetType
    {
        Prey = 0,
        Owner,
        AllPreys,
        All
    }

    public string Name;
    public ActionType Type;
    public TargetType Target;

    public bool Negative;
    public int Amount;
    public string BuffName;

    #region FieldFunctions
    public override List<string> GetFieldNames()
    {
        List<string> s = base.GetFieldNames();
        string[] addition = new string[] {
            "Name", "Type", "Target", "Negative", "Amount", "BuffName"
        };
        s.AddRange(addition);
        return s;
    }
    #endregion
}
