using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePickUp : LootItem
{
    protected override void PickUp()
    {
        //调用玩家的导弹领取函数
        player.PickUpMissile();
        base.PickUp();
    }
}
