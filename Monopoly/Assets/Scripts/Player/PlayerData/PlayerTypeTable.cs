using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomScriptableObject/PlayerTypeTable")]
public class PlayerTypeTable : GameDataTableBase<PlayerTypeData>
{
    public List<PlayerTypeData> PlayerTypeList
    {
        get
        {
            return DataList;
        }
    }
}
