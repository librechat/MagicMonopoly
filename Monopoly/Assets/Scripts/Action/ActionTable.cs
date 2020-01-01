using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "CustomScriptableObject/ActionTable")]
public class ActionTable : GameDataTableBase<ActionData>
{
    public List<ActionData> ActionDataList { get { return DataList; } }
}
