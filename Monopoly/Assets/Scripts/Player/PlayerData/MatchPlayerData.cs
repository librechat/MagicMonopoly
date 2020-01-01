using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomScriptableObject/MatchPlayerData")]
public class MatchPlayerData : GameDataTableBase<PlayerData>
{
    public List<PlayerData> PlayerList
    {
        get
        {
            return DataList;
        }
    }
}