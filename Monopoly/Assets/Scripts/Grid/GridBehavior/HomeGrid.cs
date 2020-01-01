using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeGrid : GridBase
{
    public int Group { get { return group; } }
    private int group = -1;
    private int homeRunBonus = 10;
    
    public override void Init(GridData data)
    {
        onPlayerSteppedEvent += onGridStepped;
        onPlayerPassedEvent += onGridPassed;

        base.Init(data);
    }

    private void onGridPassed(Player player)
    {
        player.Coins += homeRunBonus;
    }

    private void onGridStepped(Player player)
    {
        player.Coins += homeRunBonus;
    }
}
