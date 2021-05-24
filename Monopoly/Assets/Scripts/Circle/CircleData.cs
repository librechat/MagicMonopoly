using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class CircleData : GameDataBase
{
    public enum CircleType
    {
        Basic,
        Lottery,
        Factory
    }

    public enum CircleActionInvoker
    {
        Prey = 0,
        Owner,
        All
    }

    public enum CircleAttribute
    {
        None,
        Fire,
        Ice,
        Wind,
        Poison,
        Heal,
        Shield,
        Electric,
        Dark
    }

    public string Name;
    public int Level;
    public CircleType Type;
    public CircleActionInvoker Target;
    public CircleAttribute Attribute;
    public List<string> ActionList;

    public List<int> PrecedingIdList;
    public List<int> SucceedingIdList;

    #region FieldFunctions
    public override List<string> GetFieldNames()
    {
        List<string> s = base.GetFieldNames();
        string[] addition = new string[] {
            "Name", "Type", "Target", "Attribute", "ActionList"
        };
        s.AddRange(addition);

        return s;
    }
    #endregion
}
