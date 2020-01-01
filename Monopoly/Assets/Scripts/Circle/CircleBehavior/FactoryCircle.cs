using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryCircle : CircleBase
{
    public override void Init(CircleData data)
    {
        base.Init(data);
    }
    public override void Trapped(Player player)
    {
        base.Trapped(player);
    }

    public void Update()
    {

    }
}