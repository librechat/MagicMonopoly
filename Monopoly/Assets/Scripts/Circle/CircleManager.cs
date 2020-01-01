using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleManager : MonoBehaviour
{
    private static CircleManager s_Instance;

    [SerializeField]
    private CircleTable m_CircleTable;

    private List<CircleBase> m_CircleList;
    private List<FactoryCircle> m_FactoryCircleList;

    private void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(this);
            return;
        }
        s_Instance = this;

        m_CircleList = new List<CircleBase>();
        m_FactoryCircleList = new List<FactoryCircle>();
    }

    public static void PutCircle(Player player, BasicGrid grid, int circleId)
    {
        // put at grid, owner is player
        s_Instance.putCircle(player, grid, circleId);
    }
    public static void RemoveCircle(BasicGrid grid)
    {
        s_Instance.removeCircle(grid);
    }
    private void putCircle(Player player, BasicGrid grid, int circleId)
    {
        CircleData data = m_CircleTable.GetDataById(circleId);
        if (data.Type == CircleData.CircleType.Basic)
        {
            m_CircleList.Add(new BasicCircle());            
        }
        else if (data.Type == CircleData.CircleType.Lottery)
        {
            m_CircleList.Add(new LotteryCircle());
        }
        else if (data.Type == CircleData.CircleType.Factory)
        {
            m_FactoryCircleList.Add(new FactoryCircle());
            m_CircleList.Add(m_FactoryCircleList[m_FactoryCircleList.Count - 1]);
        }
        m_CircleList[m_CircleList.Count - 1].Init(data);
        m_CircleList[m_CircleList.Count - 1].SetAt(player, grid);
    }
    private void removeCircle(BasicGrid grid)
    {
        grid.AttachedCircle.Remove();
        m_CircleList.Remove(grid.AttachedCircle);
    }

    public void UpdateFactoryCircles()
    {
        // update factory circles every round
        for (int i = 0; i < m_FactoryCircleList.Count; i++)
        {
            m_FactoryCircleList[i].Update();
        }
    }
}
