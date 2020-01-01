using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpell : SpellBase
{
    public int CircleId { get { return m_CircleId; } }
    private int m_CircleId;

    public CircleSpell(SpellData data)
        : base(data)
    {
        m_CircleId = data.SpellEventId;
    }
    public CircleSpell(CircleSpell spell)
        : base(spell)
    {
        m_CircleId = spell.m_CircleId;
    }

    public override void Invoke(Player player)
    {
        BasicGrid basic = GridManager.GetGrid(player.Position) as BasicGrid;
        if(basic != null) CircleManager.PutCircle(player, basic, m_CircleId);

        base.Invoke(player);
    }
}
