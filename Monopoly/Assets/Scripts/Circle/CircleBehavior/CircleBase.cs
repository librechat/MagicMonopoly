using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBase
{
    public int CircleId { get { return m_Data.Id; } }
    public int Level { get { return m_Data.Level; } }
    public CircleData.CircleType Type { get { return m_Data.Type; } }
    public CircleData.CircleActionInvoker Target { get { return m_Data.Target; } }
    public CircleData.CircleAttribute Attribute { get { return m_Data.Attribute; } }
    public List<int> SucceedingIdList { get { return m_Data.SucceedingIdList; } }
    
    protected CircleData m_Data;

    public int OwnerId
    {
        get
        {
            if (m_Owner == null) return -1;
            else return m_Owner.PlayerId;
        }
    }
    public Player Owner { get { return m_Owner; } }
    protected Player m_Owner = null;

    public BasicGrid Position { get { return m_Position; } }
    protected BasicGrid m_Position = null;
    
    public virtual void Init(CircleData data)
    {
        m_Owner = null;
        m_Data = data;
    }
    public virtual void SetAt(Player player, BasicGrid grid)
    {
        m_Owner = player;
        m_Position = grid;

        grid.AttachedCircle = this;
        player.OwnCircles(this);
    }
    public virtual void Trapped(Player player)
    {

    }

    public void Remove()
    {
        m_Position.AttachedCircle = null;
        m_Owner.RemoveCircles(this);
    }
}
