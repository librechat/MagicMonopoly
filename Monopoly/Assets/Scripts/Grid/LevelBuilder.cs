using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField]
    private GridTypeTable m_Table;

    [SerializeField]
    bool useJsonFile;
    [SerializeField]
    string fileName;
    [SerializeField]
    LevelData data;

    public List<GridBase> GetLevel(Action<List<GridBase>> renderGridsEvent)
    {
        List<GridData> gridDataList;
        if (useJsonFile) gridDataList = loadLevel(fileName);
        else gridDataList = data.GridList;

        List<GridBase> gridList = buildLevel(gridDataList, renderGridsEvent);

        return gridList;
    }

    private List<GridBase> buildLevel(List<GridData> gridDataList, Action<List<GridBase>> renderGridsEvent)
    {
        List<GridBase> gridList = new List<GridBase>();

        for (int i = 0; i < gridDataList.Count; i++)
        {
            if (gridDataList[i].TypeName == GridTypeData.GridTypeName.Home)
            {
                HomeGrid home = new HomeGrid();
                home.Init(gridDataList[i]);
                gridList.Add(home);
            }
            if (gridDataList[i].TypeName == GridTypeData.GridTypeName.Basic)
            {
                BasicGrid basic = new BasicGrid();
                basic.Init(gridDataList[i]);
                gridList.Add(basic);
            }
        }

        renderGridsEvent(gridList);
        return gridList;
    }

    private List<GridData> loadLevel(string path)
    {
        path = Application.streamingAssetsPath + path;
        Debug.Log(path);
        
        string json = File.ReadAllText(path);
        LevelData result = ScriptableObject.CreateInstance<LevelData>();
        JsonUtility.FromJsonOverwrite(json, result);
        return result.GridList;
    }
    private void saveLevel()
    {

    }

}
