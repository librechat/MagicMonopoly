using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRendererManager : MonoBehaviour
{
    [SerializeField]
    private GridTypeTable m_Table; // should rename as prefab table
    
    private List<GridRendererBase> m_GridList;

    public void Init(GridManager manager)
    {
        manager.onInitGridsRenderEvent += initGrids;
    }

    private void initGrids(List<GridBase> gridList)
    {
        Dictionary<GridTypeData.GridTypeName, GridTypeData> gridTypeDic = m_Table.GetDataDicEnum();
        
        m_GridList = new List<GridRendererBase>();
        
        var level = new GameObject();
        level.name = "Level";

        for (int i = 0; i < gridList.Count; i++)
        {
            Transform prefab = gridTypeDic[gridList[i].Type].Prefab;

            GridRendererBase grid = Instantiate(prefab).GetComponent<GridRendererBase>();
            grid.Init(gridList[i]);
            m_GridList.Add(grid);

            m_GridList[i].transform.parent = level.transform;
        }
    }
    public Vector3 GetGridPos(int gridPos)
    {
        return m_GridList[gridPos].transform.position;
    }
    public List<Vector3> GetGridsPos(List<int> gridPosList)
    {
        List<Vector3> gridPos = new List<Vector3>();
        for (int i = 0; i < gridPosList.Count; i++)
        {
            gridPos.Add(m_GridList[gridPosList[i]].transform.position);
        }
        return gridPos;
    }
    public void PassedGrid(int gridPos)
    {
        m_GridList[gridPos].PassedGrid();
    }
    public void SteppedGrid(int gridPos)
    {
        m_GridList[gridPos].SteppedGrid();
    }
}
