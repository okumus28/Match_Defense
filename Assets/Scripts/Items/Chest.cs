using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : IBuilding
{
    public int moveCount;

    public static event Action<int> OnChest;

    public void OpenChest()
    {
        GameSignals.OnUpdateMoveCount(+moveCount);
    }

    protected override void AttackTarget()
    {
        return;
    }

    protected override void BuildingMatch()
    {
        return;
    }

    protected override void FindTarget()
    {
        return;
    }
}
