using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySpell : SpellBase
{
    public DestroySpell(SpellData data)
        : base(data)
    {
        
    }

    public override void Invoke(Player player)
    {
        BasicGrid basic = GridManager.GetGrid(player.Position) as BasicGrid;
        if (basic != null && basic.AttachedCircle != null)
        {
            CircleManager.RemoveCircle(basic);
        }

        base.Invoke(player);
    }
}
