using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBase
{
    public GridTypeData.GridTypeName Type { get { return m_Type; } }
    protected GridTypeData.GridTypeName m_Type;

    public int GridId { get { return m_GridId; } }
    protected int m_GridId;
    protected Vector3 m_RenderPos;
    public Vector3 RenderPos { get { return m_RenderPos; } }

    public int BranchCount { get { return m_NextGridIdList.Count; } }
    protected List<int> m_NextGridIdList;
    protected List<int> m_PreviousGridIdList;

    protected List<int> m_Occupiers;

    protected Action<Player> onPlayerPassedEvent;
    protected Action<Player> onPlayerSteppedEvent;

    public virtual void Init(GridData data)
    {
        m_Type = data.TypeName;
        m_GridId = data.Id;

        m_PreviousGridIdList = data.PreviousGridIdList;
        m_NextGridIdList = data.NextGridIdList;

        m_RenderPos = data.RenderPos;

        m_Occupiers = new List<int>();
    }

    public int GetNextGridIndex(int branch = -1)
    {
        int count = m_NextGridIdList.Count;
        if (count == 0)
        {
            Debug.LogWarning("A grid without next step");
            return -1;
        }
        else if (count == 1) return m_NextGridIdList[0];
        else
        {
            if (branch == -1)
            {
                // int r = UnityEngine.Random.Range(0, count);
                // return m_NextGridIdList[r];
                Debug.LogWarning("Please assign a branch");
                return -1;
            }
            else
            {
                return m_NextGridIdList[branch];
            }
        }
    }

    public void OnPlayerLeaved(Player player)
    {
        m_Occupiers.Remove(player.PlayerId);
    }
    public void OnPlayerPassed(Player player)
    {
        if(onPlayerPassedEvent != null) onPlayerPassedEvent(player);
    }
    public void OnPlayerStepped(Player player)
    {
        m_Occupiers.Add(player.PlayerId);
        if (onPlayerSteppedEvent != null) onPlayerSteppedEvent(player);
    }
}
