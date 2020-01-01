using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicGrid : GridBase
{
    public int OwnerId
    {
        get
        {
            if (m_AttachedCircle == null) return -1;
            else return m_AttachedCircle.OwnerId;
        }
    }
    
    public CircleBase AttachedCircle
    {
        get { return m_AttachedCircle; }
        set {
            if (value != null) onCircleChangedRenderEvent(value.Level, value.Attribute, value.Owner.Color);
            else onCircleChangedRenderEvent(-1, CircleData.CircleAttribute.None, m_AttachedCircle.Owner.Color);

            m_AttachedCircle = value;
        }
    }
    private CircleBase m_AttachedCircle;

    public Action<int, CircleData.CircleAttribute, Color> onCircleChangedRenderEvent;

    public override void Init(GridData data)
    {
        base.Init(data);
    }
}
