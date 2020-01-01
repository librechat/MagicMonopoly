using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public List<GridBase> GridList { get { return m_GridList; } }
    private List<GridBase> m_GridList;

    [SerializeField]
    private LevelBuilder m_LevelBuilder;

    #region RenderingCallbacks
    public Action<List<GridBase>> onInitGridsRenderEvent;
    #endregion

    private static GridManager s_Instance;

    #region MonoBehavior Functions
    private void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(this);
            return;
        }
        s_Instance = this;
    }
    #endregion

    public static void Init()
    {
        s_Instance.initGrids();
    }

    private void initGrids()
    {
        //s_Instance.m_GridList = m_LevelBuilder.GetLevel();
        //onInitGridsRenderEvent(s_Instance.m_GridList);

        s_Instance.m_GridList = m_LevelBuilder.GetLevel(onInitGridsRenderEvent);
    }

    public static GridBase GetGrid(int gridId)
    {
        return s_Instance.m_GridList[gridId];
    }

    public static List<HomeGrid> GetHomeGrids(){
        return s_Instance.m_GridList
            .Where( grid => grid.Type == GridTypeData.GridTypeName.Home)
            .Select( grid => grid as HomeGrid ).ToList();
    }
}
