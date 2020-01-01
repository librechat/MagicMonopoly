using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCircle : CircleBase
{
    public override void Init(CircleData data)
    {
        base.Init(data);
    }
    public override void Trapped(Player player)
    {
        base.Trapped(player);
        
        //Debug.Log("Trapped!!!" + player.PlayerId);
        if ((m_Data.Target == CircleData.CircleActionInvoker.Prey && player.PlayerId != m_Owner.PlayerId) ||
            (m_Data.Target == CircleData.CircleActionInvoker.Owner && player.PlayerId == m_Owner.PlayerId) ||
            (m_Data.Target == CircleData.CircleActionInvoker.All))
        {
            for (int i = 0; i < m_Data.ActionList.Count; i++)
            {
                ActionManager.InvokeAction(m_Data.ActionList[i], player);
            }
        }
    }
}