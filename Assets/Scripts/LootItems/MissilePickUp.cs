using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 导弹战利品
/// </summary>
public class MissilePickUp : LootItem
{
    protected override void PickUp()
    {
        player.PickUpMissile();

        base.PickUp();
    }
}
