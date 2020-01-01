using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData: GameDataBase
{
    public string Name;
    public int Group;

    [Range(0,4)]
    public int ColorId;

    // public PlayerTypeData Config;
    public int PlayerTypeId;
}
