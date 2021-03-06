﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffData: GameDataBase
{
    public enum BuffType
    {
        Action,
        Shield,
        Restrict
    }
    public enum BuffTiming
    {
        PreStage,
        Move,
        MoveStep,
        Attacked,
        Spell,
        PostStage
    }
    public enum BuffLife
    {
        OneRound,
        Level,
        Shield,
        Persistant
    }
    public enum BuffEffect
    {
        Fixed,
        Level
    }

    public string Name;

    public BuffType Type;
    public BuffTiming Timing;
    public BuffLife Life;
    public BuffEffect Effect;

    public bool Negative;
    public string BuffActionName;

    #region FieldFunctions
    public override List<string> GetFieldNames()
    {
        List<string> s = base.GetFieldNames();
        string[] addition = new string[] {
            "Name", "Type", "Timing", "Life", "Effect", "Negative", "BuffActionName"
        };
        s.AddRange(addition);
        return s;
    }
    #endregion
}
