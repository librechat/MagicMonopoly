using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomScriptableObject/BuffTable")]
public class BuffTable: GameDataTableBase<BuffData>
{
    public List<BuffData> BuffDataList { get { return DataList; } }
}
