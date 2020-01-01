using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomScriptableObject/LevelData")]
public class LevelData : GameDataTableBase<GridData>
{
    public List<GridData> GridList {
        get
        {
            return DataList;
        }
    }
}
