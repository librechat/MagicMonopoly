using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "CustomScriptableObject/CircleTable")]
public class CircleTable : GameDataTableBase<CircleData>
{
    public List<CircleData> CircleDataList { get { return DataList; } }
}