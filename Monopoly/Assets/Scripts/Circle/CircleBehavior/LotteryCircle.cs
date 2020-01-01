using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotteryCircle : CircleBase
{
    public override void Init(CircleData data)
    {
        base.Init(data);
    }
    public override void Trapped(Player player)
    {
        base.Trapped(player);

        if ((m_Data.Target == CircleData.CircleActionInvoker.Prey && player.PlayerId != m_Owner.PlayerId) ||
            (m_Data.Target == CircleData.CircleActionInvoker.Owner && player.PlayerId == m_Owner.PlayerId) ||
            (m_Data.Target == CircleData.CircleActionInvoker.All))
        {
            // conduct events randomly
            int r = Random.Range(0, m_Data.ActionList.Count);
            ActionManager.InvokeAction(m_Data.ActionList[r], player);
        }
    }
}