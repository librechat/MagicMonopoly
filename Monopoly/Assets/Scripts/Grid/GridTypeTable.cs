using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomScriptableObject/GridTypeTable")]
public class GridTypeTable : GameDataTableBase<GridTypeData>
{
    public List<GridTypeData> GridTypeDataList { get { return DataList; } }

    public Dictionary<GridTypeData.GridTypeName, GridTypeData> GetDataDicEnum()
    {
        Dictionary<GridTypeData.GridTypeName, GridTypeData> dic = new Dictionary<GridTypeData.GridTypeName,GridTypeData>();
        for(int i=0; i<GridTypeDataList.Count; i++){
            dic.Add(GridTypeDataList[i].Name, GridTypeDataList[i]);
        }
        return dic;
    }
}


